using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;


namespace RESTfulWebAPITask1
{
    public class RabbitMqPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private const string ExchangeName = "catalog_exchange";
        private const string RoutingKey = "catalog.item.updated";

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // declare exchange and make it as durable
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true);
        }

        public void PublishCatalogItemUpdatedEvent(ItemUpdatedEvent catalogEvent)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(catalogEvent));

            _channel.BasicPublish(exchange: ExchangeName,
                                  routingKey: RoutingKey,
                                  basicProperties: null,
                                  body: body);

            Console.WriteLine($"Catalog update event published: ItemId = {catalogEvent.ItemId}, Name = {catalogEvent.Name}, Price = {catalogEvent.Price}");
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

    }
}
