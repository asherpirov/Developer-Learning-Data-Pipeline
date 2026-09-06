from confluent_kafka import Producer, Consumer
import json
import os

def delivery_report(err, msg):
    if err is not None:
        print(f"Error delivery failed: {err}")
    else:
        print(f"Processed message sent to: {msg.topic()}")

def process_record(raw_dict):
    years_raw = raw_dict.get("YearsCode")
    years_code = None

    if years_raw not in [None, "","NA", "NaN", "nan"]:
        try:
            years_code = int(float(years_raw))
        except (ValueError, TypeError):
            years_code = None

    if years_code is None:
        experience_level = "Unknown"
    elif years_code <= 2:
        experience_level = "Beginner"
    elif years_code <= 5:
        experience_level = "Early Career"
    elif years_code <= 10:
        experience_level = "Experienced"
    else:
        experience_level = "Highly Experienced"

    processed = {
        "responseId": raw_dict.get("ResponseId"),
        "age": raw_dict.get("Age"),
        "yearsCode": years_code,
        "experienceLevel": experience_level,
        "devType": raw_dict.get("DevType"),
        "usesDocumentation": raw_dict.get("UsesDocumentation"),  # או איך שזה נקרא ב-CSV
        "usesAIForLearning": raw_dict.get("UsesAIForLearning"),
        "aiAcc": raw_dict.get("AiAcc"),
        "status": "processed"
    }
    return processed


def main():
    bootstrap_servers = os.environ.get("KAFKA_BOOTSTRAP_SERVERS", "localhost:9092")

    consumer_conf = {
        'bootstrap.servers': bootstrap_servers,
        'group.id': 'processing_service_group',
        'auto.offset.reset': 'earliest'
    }

    producer_conf = {
        'bootstrap.servers': bootstrap_servers
    }

    consumer = Consumer(consumer_conf)
    producer = Producer(producer_conf)

    input_topic = "raw_topic"
    output_topic = "processed_topic"

    consumer.subscribe([input_topic])
    print(f"Processing service running... Listening on '{input_topic}' -> Publishing to '{output_topic}'")

    try:
        processed_count = 0
        while True:
            msg = consumer.poll(1.0)
            if msg is None:
                continue
            if msg.error():
                print(f"Consumer error: {msg.error()}")
                continue

            raw_value = msg.value().decode('utf-8')
            raw_dict = json.loads(raw_value)

            processed_dict = process_record(raw_dict)

            processed_value = json.dumps(processed_dict).encode('utf-8')
            producer.produce(topic=output_topic, value=processed_value, callback=delivery_report)

            producer.poll(0)
            processed_count += 1
        print(f"--> Total processed so far: {processed_count}")
    except KeyboardInterrupt:
        print(f"Stopping processing service...Total processed: {processed_count}")
    finally:
        consumer.close()
        producer.flush()


if __name__ == "__main__":
    main()