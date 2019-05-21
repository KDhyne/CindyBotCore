using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CindyBotCore.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public CommandHandler(DiscordSocketClient pClient, CommandService pCommands)
        {
            _commands = pCommands;
            _client = pClient;
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;

            // Here we discover all of the command modules in the entry
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information/

            Console.WriteLine("Adding modules");
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage pMessageParam)
        {
            // Don't process the command if it was a system message
            var lMessage = pMessageParam as SocketUserMessage;
            if (lMessage == null)
            {
                return;
            }

            // Create a number to track where the prefix ends and the command begins
            int pArgPos = 0;

            // Determine if the message is a command based on the prefix and 
            // make sure no bots trigger commands
            if (!(lMessage.HasCharPrefix('!', ref pArgPos) ||
                lMessage.HasMentionPrefix(_client.CurrentUser, ref pArgPos) ||
                lMessage.Author.IsBot))
            {
                return;
            }

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, lMessage);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value,
            // rather an object stating if the command executed successfully.
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: pArgPos,
                services: null);

            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired, 
            // as it may clog up the request queue should a user spam a
            // command.
            //if (!result.IsSuccess)
            //{
            //    await context.Channel.SendMessageAsync(result.ErrorReason);
            //}
        }
    }
}
