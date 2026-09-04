using Confluent.Kafka;
using DataConsumer.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false).Build();

        var kafkaSettings = configuration.GetSection("KafkaSettings").Get<KafkaSettings>();

        var bootstrapsServers = kafkaSettings.BootstrapServers;
        var groupId = kafkaSettings.GroupId;
        var autoOffsetReset = kafkaSettings.AutoOffsetReset;
        var processedTopic = kafkaSettings.TopicName;

        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapsServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
            
        };

        var consumer = new ConsumerBuilder<Ignore,string>(config).Build();

        consumer.Subscribe(processedTopic);

        while (true)
        {
            try
            {
                var result = consumer.Consume(TimeSpan.FromSeconds(15));

                if (result == null || result.Message?.Value == null)
                {
                    continue;
                }

                var processed = JsonSerializer.Deserialize<ProcessedMessage>(result.Message.Value);
                Console.WriteLine($"Received message with ID: {processed.ResponseId}");


            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e.Message}");
            }
        }

    }
}
