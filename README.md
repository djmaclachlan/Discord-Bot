# Discord Bot
  # A Discord bot written in c# with winforms.

BotModule.cs contains automated responses to messages from discord, I left in the basic ping/pong response as well as a 20 sided dice roller when !roll is entered.

MainBotForm.cs is the actual winform UI. Within it there is a player/npc tracker (Used for Role Playing Games) that retains data through an MS Acess file. This will be changed to sqlite, but for quick testing that this would even work I just threw together a few tables. 

Using this application is very easy, enter your token and hit connect to connect to your server. Copy and paste the channel ID into the appropriate text box and you are all set. All UI elements (dice roll, message, cypher generate, etc) will all be bound to the channel ID you have put in. 

As development continues, I will implement an auto channel fetch for the server token input and populate a list for quick access. At the moment it is eluding me as to how to do it, hence the manual input. 
