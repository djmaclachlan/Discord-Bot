# Discord Bot
  A Discord bot written on a winforms application

BotModule.cs contains automated responses to messages from discord, I left in the basic ping/pong response as well as a 20 sided dice roller when !roll is entered.

MainBotForm.cs is the actual winform UI. Within it there is a player/npc tracker (Used for Role Playing Games) that retains data through an MS Acess file. This will be changed to sqlite, but for quick testing that this would even work I just threw together a few tables. To demonstrate that the UI is able to interact with a discord guild/channel, I have included a custom dice roller available to the admin. You will find 
            var channel = Client.GetChannel("Insert-Channel-ID-Here") as SocketTextChannel;
            await channel.SendMessageAsync("The GM has rolled a " + sides + " sided die and the result is [" + output + "]");
            
on line 65/66 of MainBotForm.cs, you need to insert the channel ID you plan on sending messages to, you can find how to get your channel ID through many other sources online. Eventually I will add a place for non-programmer users to just input this on the form.

The only remaining thing a user needs to know is that on the UI there is a text box, simply enter your token in the box and press connect to get the bot up and running.

This is a side project and as such will be worked on when I have time, It is completely open source and I encourage others to make functionality requests so I have more ideas to run with and of course contributions will be met with thank yous and praise!
