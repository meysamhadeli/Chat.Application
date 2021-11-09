using System;
using System.Text;
using Microsoft.Extensions.Options;
using NATS.Client;
using Newtonsoft.Json;

namespace Chat.Core.Infrastructure.Nats
{
    public class NatsBus : INatsBus, IDisposable
    {
        private readonly IConnection _connection;

        public NatsBus(IOptions<NatsOptions> natsOptions)
        {
            ConnectionFactory factory = new ConnectionFactory();

            var options = ConnectionFactory.GetDefaultOptions();
            options.Url = natsOptions.Value.Address; //"nats://localhost:4222";
            _connection = factory.CreateConnection(options);
        }

        public IAsyncSubscription Subscribe<T>(Action<T> handler, string subjectName) where T : class
        {
            IAsyncSubscription subscription = _connection.SubscribeAsync(subjectName, (sender, args) =>
            {
                string json = Encoding.UTF8.GetString(args.Message.Data);
                var data = JsonConvert.DeserializeObject<T>(json);
                handler(data);
            });

            return subscription;
        }

        public void Publish<T>(T message, string subjectName) where T : class
        {
            var data = JsonConvert.SerializeObject(message);
            _connection.Publish(subjectName, Encoding.UTF8.GetBytes(data));
        }

        public void Dispose()
        {
            // Draining and closing a connection
            _connection.Drain();

            // Closing a connection
            _connection.Close();
        }
    }
}