using System;
using NATS.Client;

namespace Chat.Core.Infrastructure.Nats
{
    public interface INatsBus
    {
        IAsyncSubscription Subscribe<T>(Action<T> handler, string subjectName) where T : class;
        void Publish<T>(T message, string subjectName) where T : class;
    }
}