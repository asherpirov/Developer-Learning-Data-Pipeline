using Confluent.Kafka;
using DataConsumer.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
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

        var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(processedTopic);

        var mongoConnectionString = configuration.GetConnectionString("MongoDb") ?? "mongodb://mongodb:27017";
        var mongoClient = new MongoClient(mongoConnectionString);

        var database = mongoClient.GetDatabase("developer_db");
        var collection = database.GetCollection<ProcessedMessage>("respondents");

        Console.WriteLine("Consumer is running, waiting for messages and saving to MongoDB...");

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
                Console.WriteLine($"Received and saving message with ID: {processed.ResponseId}");

                collection.InsertOne(processed);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}