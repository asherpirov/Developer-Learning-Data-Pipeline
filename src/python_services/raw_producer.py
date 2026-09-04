from confluent_kafka import Producer
import time
import csv
import json


def delivery_report(err, msg):
    if err is not None:
        print(f"Error delivering message: {err}")
    else:
        print(f"Send to:", msg.topic())

def get_kafka_producer():
    config = {"bootstrap.servers": "localhost:9092"}
    return Producer(config)

def produce_from_csv(filepath, producer, topicname):
    count = 0
    with open(filepath, "r", encoding="utf-8") as f:
        reader = csv.DictReader(f)
        for row in reader:
            value = json.dumps(row)
            producer.produce(topic=topicname, value=value, callback=delivery_report)
            producer.poll(0)
            count += 1

    print("Flushing records...")
    producer.flush()
    print("Messages Number:",count)

def main():

    filepath = "data/developer_ai_learning_raw.csv"
    topic_name = "raw_topic"

    producer = get_kafka_producer()
    produce_from_csv(filepath,producer,topic_name)

if __name__ == "__main__":
    main()