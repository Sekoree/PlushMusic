# PlushMusic
"Ready to use" Discord music bot using Lavalink

this is basically a complete standalsone version of the music systems that MeekPlush and Hatsune Miku use
## What you need
- [Lavalink 3](https://github.com/Frederikam/Lavalink)<br>
  -> this needs Java 10, make sure its installed!
- [Discord Bot Token](https://discordapp.com/developers/applications/)
- (optional) Youtube API Token

## Setup
1. Make a new Application in Discords Dev Panel, and make a bot user<br>
  -> invite the bot to your server with a link like this<br>
  -> https://discordapp.com/oauth2/authorize?client_id=BOT-CLIENT-IDscope=bot&permissions=104197184<br>
  (replace thet BOT-CLIENT-ID with the ID of your bot)
2. Start up a Lavalink 3 server, use the [example config](https://github.com/Frederikam/Lavalink/blob/master/LavalinkServer/application.yml.example) (edit that to your needs and desires)<br>
  -> make a file called application.yml in the same directory as the Lavalink.jar and paste the stuff from the link in there
3. Edit the config.json<br>
  -> if no Youtube API Key is provided, extended track info in the "nowplaying" command will be disabled
4. Use the Start.bat to start the bot!
