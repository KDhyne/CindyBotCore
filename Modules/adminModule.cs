using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CindyBot.Modules
{
    [Group("admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Group("clean")]
        public class CleanModule : ModuleBase<SocketCommandContext>
        {
            // !admin clean
            [Command]
            public async Task DefaultCleanAsync()
            {
                // .. Do some cleaning with default options
                await ReplyAsync("```Not yet implemented```");
            }
            
            // !admin clean
            [Command("messages")]
            [Summary("Clean the speficied number of messages in this channel. Input -1 to clear all messages")]
            public async Task CleanMessagesAsync(int count)
            {
                if (count < -1)
                {
                    await ReplyAsync("```Invalid input was given```");
                    return;
                }

                var messages = await Context.Channel.GetMessagesAsync(count).FlattenAsync();
                foreach (var m in messages)
                {
                    await Context.Channel.DeleteMessageAsync(m);
                }
                await ReplyAsync($"```{count} message{(count != 1 ? "s" : "")} deleted```");
            }

            [Command("bot")]
            [Summary("Scan the last 100 messages in the given channel and delete any posted by the bot. Defaults to the current channel")]
            public async Task CleanBotMessages(string channelName = "")
            {
                var TargetChannel = Context.Channel as IMessageChannel;
                var self = await TargetChannel.GetUserAsync(425269902497153024);

                if (channelName != "")
                {
                    TargetChannel = Context.Guild.TextChannels.FirstOrDefault(c => c.Name == channelName);
                }

                var messageChunk = await TargetChannel?.GetMessagesAsync(100).FlattenAsync();
                var botMessageCount = 0;

                foreach (IMessage m in messageChunk)
                {
                    if (m.Author == self)
                    {
                        botMessageCount++;
                        await TargetChannel?.DeleteMessageAsync(m);
                    }
                }
                await ReplyAsync($"```{botMessageCount} bot message{(botMessageCount != 1 ? "s" : "")} deleted```");
            }

            

            [Command("channel")]
            [Summary("Delete all messages in the given channel. Defaults to the current channel")]
            public async Task CleanChannelMessages(string channelName = "")
            {
                var TargetChannel = Context.Channel as IMessageChannel;

                if (channelName != "")
                {
                    TargetChannel = Context.Guild.TextChannels.FirstOrDefault(c => c.Name == channelName);
                }

                var messageChunk = await TargetChannel?.GetMessagesAsync(100).FlattenAsync();
                var botMessageCount = 0;

                foreach (IMessage m in messageChunk)
                {
                        botMessageCount++;
                        await TargetChannel?.DeleteMessageAsync(m);
                }
                await ReplyAsync($"```{botMessageCount} bot message{(botMessageCount != 1 ? "s" : "")} deleted```");
            }
        }
    }
}