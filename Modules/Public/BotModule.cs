using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotApplication.Modules.Public
{
    public class BotModule : ModuleBase
    {
        [Command("ping", RunMode = RunMode.Async)]

        public async Task SendPong()
        {
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}, Pong!");
        }

        [Command("roll", RunMode = RunMode.Async)]
        public async Task RollDice()
        {
            Random rand = new Random();
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}, " + rand.Next(1, 20));
        }
    }
    
}
