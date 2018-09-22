using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using DSharpPlus.CommandsNext.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Threading;
using DSharpPlus.Net.Udp;
using Newtonsoft.Json;
using System.Collections.Generic;
using DSharpPlus.CommandsNext.Attributes;
using BetaPlush.Commands.MusicEx;
using static PlushMusic.Program;

namespace MikuMusicSharp.BotClass.BotNew
{
    public class Bot : IDisposable
    {
        private DiscordShardedClient bot;
        private CancellationTokenSource _cts;
        public static LavalinkConfiguration lcfg = new LavalinkConfiguration
        {
            Password = config.LavaLinkPassword,
            SocketEndpoint = new ConnectionEndpoint { Hostname = config.LavaLinkIP, Port = config.SocketPort },
            RestEndpoint = new ConnectionEndpoint { Hostname = config.LavaLinkIP, Port = config.RestPort }
        };
        public static List<Gsets> guit = new List<Gsets>();
        public int ok = 0;

        public Bot()
        {
            bot = new DiscordShardedClient(new DiscordConfiguration()
            {
                LogLevel = LogLevel.Debug,
                Token = config.Token,
                TokenType = TokenType.Bot,
                AutomaticGuildSync = true,
                UseInternalLogHandler = true,
                AutoReconnect = true
            });

            _cts = new CancellationTokenSource();
            bot.Ready += OnReadyAsync;
        }

        public async Task RunAsync()
        {
            await bot.StartAsync();

            var commands = await bot.UseCommandsNextAsync(new CommandsNextConfiguration()
            {
                StringPrefixes = (new[] { config.Prefix }),
                EnableDefaultHelp = true,
                IgnoreExtraArguments = false,
                CaseSensitive = false
            });
            var interactivity = await bot.UseInteractivityAsync(new InteractivityConfiguration { });
            var llink = await bot.UseLavalinkAsync();
            foreach (var cmd in commands)
            {
                cmd.Value.RegisterCommands<BetaPlush.Commands.Music>();
                //cmd.Value.RegisterCommands<BetaPlush.Commands.PixivTest>();
                cmd.Value.CommandErrored += Bot_CMDErr;
            }
            bot.ClientErrored += this.Bot_ClientErrored;

            guit.Add(new Gsets
            {
                GID = 0,
                prefix = new List<string>(new string[] { "m%" }),
                LLinkCon = await llink.First().Value.ConnectAsync(lcfg)
            });

            foreach (var shard in bot.ShardClients)
            {
                shard.Value.VoiceStateUpdated += async e =>
                {
                    try
                    {
                        var con = guit[0].LLinkCon;
                        var pos = guit.FindIndex(x => x.GID == e.Guild.Id);
                        if (pos == -1 || !con.IsConnected || con == null) { await Task.CompletedTask; return; }
                        var norm = e?.Channel?.Id;
                        var afte = e?.After?.Channel?.Id;
                        var befo = e?.Before?.Channel?.Id;
                        if (norm == guit[pos].LLGuild?.Channel?.Id || afte == guit[pos].LLGuild?.Channel?.Id || befo == guit[pos].LLGuild?.Channel?.Id)
                        {
                            if (guit[pos].LLGuild?.Channel?.Users.Where(x => !x.IsBot).Count() == 0)
                            {
                                guit[pos].paused = true;
                                await Task.Run(() => guit[pos].AudioFunctions.Pause(pos));
                                await e.Guild.GetChannel(guit[pos].cmdChannel).SendMessageAsync($"Playback was paused since everybody left the channel! use ``{config.Prefix}resume`` to resume, otherwise I'll also disconnect in ~5min uwu");
                                var haDi = handleVoidisc(pos);
                                haDi.Wait(millisecondsTimeout: 2500);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                    await Task.CompletedTask;
                };
                shard.Value.GuildCreated += async e =>
                {
                    var pos = guit.FindIndex(x => x.GID == e.Guild.Id);
                    if (pos == -1)
                    {
                        guit.Add(new Gsets
                        {
                            GID = e.Guild.Id,
                            prefix = new List<string>(new string[] { "m%" }),
                            queue = new List<Gsets2>(),
                            playnow = new Gsets3(),
                            repeat = false,
                            offtime = DateTime.Now,
                            shuffle = false,
                            LLGuild = null,
                            playing = false,
                            rAint = 0,
                            repeatAll = false,
                            AudioEvents = new LLEvents(),
                            AudioFunctions = new Functions(),
                            AudioQueue = new Queue(),
                            sstop = false,
                            paused = false
                        });
                        ok++;
                    }
                    await Task.CompletedTask;
                };

                shard.Value.GuildDeleted += async e =>
                {
                    ok--;
                    await Task.CompletedTask;
                };

                shard.Value.GuildDownloadCompleted += async e =>
                {
                    Console.WriteLine("GD!");
                    UpdatePre().Wait(500);
                    await Task.Delay(500);
                    //foreach (var shard in bot.ShardClients)
                    //{
                        //Console.WriteLine("Shards!");
                        foreach (var guilds in shard.Value.Guilds)
                        {
                            //Console.WriteLine("Guild!");
                            guit.Add(new Gsets
                            {
                                GID = guilds.Value.Id,
                                prefix = new List<string>(new string[] { "%" }),
                                queue = new List<Gsets2>(),
                                playnow = new Gsets3(),
                                repeat = false,
                                offtime = DateTime.Now,
                                shuffle = false,
                                LLGuild = null,
                                playing = false,
                                rAint = 0,
                                repeatAll = false,
                                AudioEvents = new LLEvents(),
                                AudioFunctions = new Functions(),
                                AudioQueue = new Queue(),
                                sstop = false,
                                paused = false
                            });
                            if (guilds.Value.Id == 336039472250748928)
                            {
                                Console.WriteLine("Derpy guild");
                            }
                            ok++;
                        //}
                    }
                    Console.WriteLine("GuildList Complete");
                    await Task.CompletedTask;
                };
            }
            /*bot.Heartbeated += async e =>
            {
                AuthDiscordBotListApi DblApi = new AuthDiscordBotListApi(483461221643845632, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQ4MzQ2MTIyMTY0Mzg0NTYzMiIsImJvdCI6dHJ1ZSwiaWF0IjoxNTM2MzUyODM5fQ.6qaJxtrXQ22FI8MrLn5m3YxOYkA_hYaKGKtK7zgWbGU");
                var me = await DblApi.GetMeAsync();
                await me.UpdateStatsAsync(ok);
                Console.WriteLine($"DBL Updated: {ok} Shards: {e.Client.ShardCount}");
            };*/
            Console.WriteLine("GD?");
            await WaitForCancellationAsync();
        }

        private async Task UpdatePre()
        {
            while (true)
            {
                DiscordActivity test = new DiscordActivity
                {
                    Name = $"{config.Prefix}help for commands!",
                    ActivityType = ActivityType.Playing
                };
                await bot.UpdateStatusAsync(activity: test, userStatus: UserStatus.Online);
                await Task.Delay(TimeSpan.FromMinutes(30));
            }
        } 

        private async Task WaitForCancellationAsync()
        {
            while (!_cts.IsCancellationRequested)
                await Task.Delay(500);
        }

        private Task<int> PreGet(DiscordMessage msg)
        {
            int pos = guit.FindIndex(x => x.GID == msg.Channel.GuildId);
            var wtf = msg.Content;
            if (pos != -1)
            {
                var multiprefloc = guit[pos].prefix.FindIndex(x => wtf.StartsWith(x));
                int prefloc;
                if (multiprefloc != -1)
                {
                    prefloc = msg.GetStringPrefixLength(guit[pos].prefix[multiprefloc]);

                    if (prefloc != -1)
                    {
                        return Task.FromResult(prefloc);
                    }
                }
            }
            return Task.FromResult(msg.GetStringPrefixLength(guit[0].prefix[0]));
        }

        private async Task OnReadyAsync(ReadyEventArgs e)
        {
            await Task.Yield();
        }

        public void Dispose()
        {
            //this.interactivity = null;
            //this.commands = null;
        }

        internal void WriteCenter(string value, int skipline = 0)
        {
            for (int i = 0; i < skipline; i++)
                Console.WriteLine();

            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
        }

        public async Task handleVoidisc(int pos) //if a message needs to be sent to another channel, in commands this is not needed
        {
            try
            {
                guit[pos].offtime = DateTime.Now;
                await Task.CompletedTask;
                while (guit[pos].LLGuild.Channel.Users.Where(x => !x.IsBot).Count() == 0 || guit[pos].queue.Count < 1)
                {
                    ///Console.WriteLine("Disc");
                    if (DateTime.Now.Subtract(guit[pos].offtime).TotalMinutes > 5)
                    {
                        guit[pos].LLGuild.PlaybackFinished -= guit[pos].AudioEvents.PlayFin;
                        guit[pos].LLGuild.TrackStuck -= guit[pos].AudioEvents.PlayStu;
                        guit[pos].LLGuild.TrackException -= guit[pos].AudioEvents.PlayErr;
                        guit[pos].LLGuild.Disconnect();
                        guit[pos].playing = false;
                        guit[pos].paused = false;
                        guit[pos].LLGuild = null;
                        guit[pos].offtime = DateTime.Now;
                        break;
                    }
                    else
                    {
                        await Task.Delay(10000);
                    }
                }
            }
            catch { }
            await Task.CompletedTask;
        }

        private async Task Bot_CMDErr(CommandErrorEventArgs ex) //if bot error
        {
            //e.Context.RespondAsync($"**Error:**\n```{e.Exception.Message}```");
            if(ex.Exception is ChecksFailedException exx){
                foreach(CheckBaseAttribute a in exx.FailedChecks){
                    if (a is CooldownAttribute cd){
                        await ex.Context.RespondAsync($"Cooldown, {(int)cd.GetRemainingCooldown(ex.Context).TotalSeconds}s left");
                    }
                    if (a is RequireBotPermissionsAttribute bpa)
                    {
                        await ex.Context.Member.SendMessageAsync($"Heya, sorry for the DM but I need to be able to ``Send Messages`` and ``Embed Links`` in order to use the music functions!");
                    }
                }
            }
            Console.WriteLine(ex.Exception.Message);
            Console.WriteLine(ex.Exception.StackTrace);
            await Task.CompletedTask;
        }

        private Task Bot_ClientErrored(ClientErrorEventArgs e) //if bot error
        {
            Console.WriteLine(e.Exception.Message);
            Console.WriteLine(e.Exception.StackTrace);
            return Task.CompletedTask;
        }

        public Task itError(CommandErrorEventArgs oof)
        {
            if (oof.Exception.HResult != -2146233088)
            {
                oof.Context.RespondAsync(oof.Command.Description); //as i explained above
            }
            return Task.CompletedTask;
        }
    }

    public class Gsets
    {
        public ulong GID { get; set; }
        public LavalinkNodeConnection LLinkCon { get; set; }
        public LavalinkGuildConnection LLGuild { get; set; }
        public List<string> prefix { get; set; }
        public List<Gsets2> queue { get; set; }
        public Gsets3 playnow { get; set; }
        public DateTime offtime { get; set; }
        public bool repeat { get; set; }
        public bool repeatAll { get; set; }
        public int rAint { get; set; }
        public bool shuffle { get; set; }
        public bool playing { get; set; }
        public ulong cmdChannel { get; set; }
        public bool sstop { get; set; }
        public bool paused { get; set; }
        public LLEvents AudioEvents { get; set; }
        public Functions AudioFunctions { get; set; }
        public Queue AudioQueue { get; set; }
    }

    public class Gsets2
    {
        public DiscordMember requester { get; set; }
        public LavalinkTrack LavaTrack { get; set; }
        public DateTime addtime { get; set; }
    }

    public class Gsets3
    {
        public DiscordMember requester { get; set; }
        public LavalinkTrack LavaTrack { get; set; }
        public DateTime addtime { get; set; }
    }
}
