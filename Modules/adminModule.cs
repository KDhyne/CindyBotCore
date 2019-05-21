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
                await ReplyAsync("Not yet implemented");
            }
            
            // !admin clean
            [Command("messages")]
            [Summary("Clean the speficied number of messages in this channel. Input -1 to clear all message")]
            public async Task CleanMessagesAsync(int count)
            {
                var messages = await Context.Channel.GetMessagesAsync(count).FlattenAsync();
                foreach (var m in messages)
                {
                    // Delete
                    await Context.Channel.DeleteMessageAsync(m);
                }
                await ReplyAsync($"{count} message{(count > 1 ? "s" : "")} deleted");
            }
        }
    }
}