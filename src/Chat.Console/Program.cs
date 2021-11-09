using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Chat.Core.Dtos;
using Chat.Core.Features.Chat.SendMessage;
using Chat.Core.Infrastructure.Nats;
using Chat.Core.Models;
using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chat.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            var host = CreateHostBuilder(args, config).Build();
            var services = host.Services;

            var apiOption = services.GetRequiredService<APIOptions>();
            var chatUiService = services.GetRequiredService<IChatUIService>();
            var bus = services.GetRequiredService<INatsBus>();
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

            System.Console.WriteLine("Your user name:");
            var userName = System.Console.ReadLine()?.Trim();
            System.Console.WriteLine("Chat with user name:");
            var targetUserName = System.Console.ReadLine()?.Trim();
            System.Console.WriteLine("Chat Started.");
            System.Console.WriteLine("");
            chatUiService.SubscribeOnChatMessage(userName, bus);
            await chatUiService.LoadReceivedMessages(userName, httpClientFactory, apiOption);

            while (true)
            {
                var inputMessage = System.Console.ReadLine();
                await chatUiService.SendMessage(userName, targetUserName, inputMessage, httpClientFactory, apiOption);
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(config => { config.ClearProviders(); })
                .ConfigureServices((_, services) =>
                    {
                        services.AddTransient<INatsBus, NatsBus>()
                            .AddTransient<IChatUIService, ChatUIService>()
                            .AddOptions<NatsOptions>().Bind(configuration.GetSection("NatsOptions"))
                            .ValidateDataAnnotations();

                        var apiOptions = configuration.GetSection("APIOptions").Get<APIOptions>();
                        services.AddSingleton(apiOptions);

                        var baseAddress = apiOptions.BaseAddress;

                        services.AddHttpClient("APIClient", config =>
                        {
                            config.BaseAddress = new Uri(baseAddress);
                            config.Timeout = new TimeSpan(0, 0, 30);
                            config.DefaultRequestHeaders.Clear();
                        });
                    }
                );
        }
    }
}