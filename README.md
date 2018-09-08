# PlushMusic
"Ready to use" Discord music bot using Lavalink

##What you need
- [Lavalink 3](https://github.com/Frederikam/Lavalink)
  -> this needs Java 10, make sure its installed!
- [Discord Bot Token](https://discordapp.com/developers/applications/)
- (optional) Youtube API Token

##Setup
1. Make a new Applications in Discords Dev Panel, and make a bot user
  -> invite the bot to your server with a link like this
  -> https://discordapp.com/oauth2/authorize?client_id=BOT-CLIENT-IDscope=bot&permissions=104197184
2. Start up a Lavalink 3 server, use the [example config](https://github.com/Frederikam/Lavalink/blob/master/LavalinkServer/application.yml.example) (edit that to your needs and desires)
  -> make a file called application.yml in the same directory as the Lavalink.jar and paste the stuff from the link in there
3. Edit the config.json
  -> if no Youtube API Key is provided, extenden track info in the "nowplaying" will be disabled
4. Use the Start.bat to start the bot!
