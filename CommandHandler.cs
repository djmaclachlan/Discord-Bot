
using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.Webhook;
using Discord.WebSocket;

namespace DiscordBotApplication
{
    class CommandHandler
    {
        private CommandService Commands { get; set; }
        private DiscordSocketClient Client { get; set; }

        public async Task Install(DiscordSocketClient Client)
        {
            this.Client = Client;
            Commands = new CommandService();

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Client.MessageReceived += HandleCommand;
        }
        private async Task HandleCommand(SocketMessage Msg)
        {
            var msg = Msg as SocketUserMessage;
            if(msg == null) return;

            int argPos = 0;
                if (!(msg.HasStringPrefix("!", ref argPos) || msg.HasMentionPrefix(Client.CurrentUser, ref argPos))) return;

            var context = new CommandContext(Client, msg);
            var result = await Commands.ExecuteAsync(context, argPos, null);

            if (!result.IsSuccess) await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
