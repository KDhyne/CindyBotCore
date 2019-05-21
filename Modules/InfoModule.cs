using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace CindyBot.Modules
{
    // Keep in mind that your module **must** be public and inherit ModuleBase.
    // If not, it will not be discovered by AddModulesAsync!

    // Create a module with no prefix
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        // ~say Hello World -> hello world
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string pEchoString)
            => ReplyAsync(pEchoString);

        // ReplyAsync is a method on ModuleBase
    }

    // Create a module with the 'sample' prefix
    [Group("sample")]
    public class SampleModule : ModuleBase<SocketCommandContext>
    {
        // !sample square 20 -> 400
        [Command("square")]
        [Summary("Squares a number.")]
        public async Task SquareAsync([Summary("The number to square")] int pNum)
        {
            await Context.Channel.SendMessageAsync($"{pNum}^2 = {Math.Pow(pNum, 2)}");
        }

        // ~sample userinfo --> foxbot#0282
        // ~sample userinfo @Khionu --> Khionu#8708
        // ~sample userinfo Khionu#8708 --> Khionu#8708
        // ~sample userinfo Khionu --> Khionu#8708
        // ~sample userinfo 96642168176807936 --> Khionu#8708
        // ~sample whois 96642168176807936 --> Khionu#8708
        [Command("userinfo")]
        [Summary("Returns info about the current user, or the user parameter if one is passed")]
        [Alias("user", "whois")]
        public async Task UserInfoAsync([Summary("The (optional) user to get info from")] SocketUser pUser = null)
        {
            var pUserInfo = pUser ?? Context.Client.CurrentUser;
            await ReplyAsync($"{pUserInfo.Username}#{pUserInfo.Discriminator}");
        }
    }
}
