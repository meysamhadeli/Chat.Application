using System.Net.Http;
using System.Threading.Tasks;
using Chat.Core.Infrastructure.Nats;

namespace Chat.Console
{
    public interface IChatUIService
    {
        Task LoadReceivedMessages(string userName, IHttpClientFactory factory, APIOptions apiOptions);
        void SubscribeOnChatMessage(string userName, INatsBus bus);

        Task SendMessage(string userName, string receiver, string message, IHttpClientFactory factory,
            APIOptions apiOptions);
    }
}