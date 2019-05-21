using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CindyBotCore
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
            _client = new DiscordSocketClient(_config);

            using (var services = ConfigureServices())
            {
                var _client = services.GetRequiredService<DiscordSocketClient>();

                _client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                // Tokens are secret and therefore loaded from the Environment Variables!
                await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
                await _client.StartAsync();

                // Here we intialize the logic required to register our commands.
                await services.GetRequiredService<Services.CommandHandler>().InstallCommandsAsync();
                
                // Block this task until the program is closed.
                await Task.Delay(-1);
            }
        }

        private Task LogAsync(LogMessage pLog)
        {
            Console.WriteLine(pLog.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> pBefore, SocketMessage pAfter, ISocketMessageChannel pChannel)
        {
            // If the message was not in the cache, downloading it will result in getting a copy of 'after'.
            var lMessage = await pBefore.GetOrDownloadAsync();
            Console.WriteLine($"{lMessage} -> {pAfter} on Channel {pChannel}");
        }

        public string GetChannelTopic(ulong pChannelID)
        {
            var lChannel = _client.GetChannel(pChannelID) as SocketTextChannel;
            return lChannel?.Topic;
        }

        public SocketGuildUser GetGuildOwner(SocketChannel pChannel)
        {
            var lGuild = (pChannel as SocketGuildChannel)?.Guild;
            return lGuild.Owner;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<Services.CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}