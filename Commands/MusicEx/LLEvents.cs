using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink.EventArgs;
using MikuMusicSharp.BotClass.BotNew;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BetaPlush.Commands.MusicEx
{
    public class LLEvents
    {
        public async Task PlayFin(TrackFinishEventArgs lg)
        {
            Console.WriteLine(lg.Reason);
            Console.WriteLine(lg.Track.Author);
            Console.WriteLine(lg.Track.Title);
            Console.WriteLine(lg.Track.Uri);
            Console.WriteLine("End Event");
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == lg.Player.Guild.Id);
            if (pos == -1 || !con.IsConnected || con == null) { await Task.CompletedTask; return; }
            if (lg.Reason == TrackEndReason.LoadFailed)
            {
                try
                {
                    Bot.guit[pos].queue.RemoveAt(Bot.guit[pos].queue.FindIndex(x => x.addtime == Bot.guit[pos].playnow.addtime));
                }
                catch { }
                await lg.Player.Guild.GetChannel(Bot.guit[pos].cmdChannel).SendMessageAsync("Track error, maybe regionlocked, skipped >>");
                Console.WriteLine($"[{lg.Player.Guild.Id}] Song errored/regionblocked");
                await Task.CompletedTask;
                return;
            }
            if (lg.Track.IsStream && lg.Reason != TrackEndReason.Stopped)
            {
                Bot.guit[pos].sstop = false;
                Bot.guit[pos].LLGuild.Play(Bot.guit[pos].playnow.LavaTrack);
                Console.WriteLine("LL Stream Error");
                await Task.CompletedTask;
                return;
            }
            if (!Bot.guit[pos].repeat && !Bot.guit[pos].repeatAll && Bot.guit[pos].LLGuild != null && !Bot.guit[pos].sstop)
            {
                Bot.guit[pos].queue.Remove(Bot.guit[pos].queue.Find(x => x.addtime == Bot.guit[pos].playnow.addtime));
            }
            if (Bot.guit[pos].sstop)
            {
                Bot.guit[pos].sstop = false;
                Bot.guit[pos].playing = false;
            }
            else if (Bot.guit[pos].queue.Count != 0)
            {
                Bot.guit[pos].playing = true;
                Bot.guit[pos].paused = false;
                await setPlay(pos);
                int nextSong = 0;
                System.Random rnd = new System.Random();
                if (Bot.guit[pos].shuffle) nextSong = rnd.Next(0, Bot.guit[pos].queue.Count);
                if (Bot.guit[pos].repeatAll) { Bot.guit[pos].rAint++; nextSong = Bot.guit[pos].rAint;
                    if (Bot.guit[pos].rAint == Bot.guit[pos].queue.Count) { Bot.guit[pos].rAint = 0; nextSong = 0; }
                }
                await setNP(pos, Bot.guit[pos].queue[nextSong]);
                Console.WriteLine($"[{lg.Player.Guild.Id}] Playing {Bot.guit[pos].playnow.LavaTrack.Title} by {Bot.guit[pos].playnow.LavaTrack.Author}");
                Bot.guit[pos].LLGuild.Play(Bot.guit[pos].playnow.LavaTrack);
                var Recover = PlayRecover(Bot.guit[pos]);
                Recover.Wait(1000);
            }
            else
            {
                Bot.guit[pos].paused = false;
                Bot.guit[pos].playing = false;
            }
            await Task.CompletedTask;
        }

        public async Task PlayRecover(Gsets bot)
        {
            Console.WriteLine("Recover engaged");
            var deadd = bot.playnow.addtime;
            var pos = Bot.guit.FindIndex(x => x.GID == bot.GID);
            var nowtime = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition;
            bool o = true;
            while (Bot.guit[pos].playing && !Bot.guit[pos].sstop)
            {
                Console.WriteLine("Waiting");
                if (o)
                {
                    o = false;
                    await Task.Delay(10000);
                }
                if (deadd != Bot.guit[pos].playnow.addtime || (Bot.guit[pos].repeat && deadd == Bot.guit[pos].playnow.addtime ))
                {
                    Console.WriteLine("Breakout");
                    break;
                }
                if (!Bot.guit[pos].paused && nowtime == Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition)
                {
                    Console.WriteLine("Stuck");
                    if (Bot.guit[pos].repeat && !Bot.guit[pos].repeatAll && Bot.guit[pos].LLGuild != null && !Bot.guit[pos].sstop)
                    {
                        Bot.guit[pos].queue.Remove(Bot.guit[pos].queue.Find(x => x.addtime == Bot.guit[pos].playnow.addtime));
                    }
                    if (Bot.guit[pos].queue.Count != 0)
                    {
                        Bot.guit[pos].playing = true;
                        Bot.guit[pos].paused = false;
                        await setPlay(pos);
                        int nextSong = 0;
                        System.Random rnd = new System.Random();
                        if (Bot.guit[pos].shuffle) nextSong = rnd.Next(0, Bot.guit[pos].queue.Count);
                        if (Bot.guit[pos].repeatAll)
                        {
                            Bot.guit[pos].rAint++; nextSong = Bot.guit[pos].rAint;
                            if (Bot.guit[pos].rAint == Bot.guit[pos].queue.Count) { Bot.guit[pos].rAint = 0; nextSong = 0; }
                        }
                        await setNP(pos, Bot.guit[pos].queue[nextSong]);
                        Console.WriteLine($"[{Bot.guit[pos].GID}] Playing {Bot.guit[pos].playnow.LavaTrack.Title} by {Bot.guit[pos].playnow.LavaTrack.Author}");
                        Bot.guit[pos].LLGuild.Play(Bot.guit[pos].playnow.LavaTrack);
                        var Recover = PlayRecover(Bot.guit[pos]);
                        Recover.Wait(1000);
                    }
                    else
                    {
                        Bot.guit[pos].paused = false;
                        Bot.guit[pos].playing = false;
                    }
                }
                else
                {
                    nowtime = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition;
                }
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
                if (Bot.guit[pos].playing) await Task.Delay(1000);
            }
        }

        public async Task PlayStu(TrackStuckEventArgs ts)
        {
            Console.WriteLine("StuckEvent");
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == ts.Player.Guild.Id);
            if (pos == -1)
            {
                await Task.CompletedTask;
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream && !Bot.guit[pos].playnow.LavaTrack.IsStream)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester,
                    addtime = Bot.guit[pos].playnow.addtime
                });
            }
            await Task.CompletedTask;
        }

        public async Task PlayErr(TrackExceptionEventArgs ts)
        {
            Console.WriteLine("ErrorEvent");
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == ts.Player.Guild.Id);
            if (pos == -1)
            {
                await Task.CompletedTask;
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream && !Bot.guit[pos].playnow.LavaTrack.IsStream)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester,
                    addtime = Bot.guit[pos].playnow.addtime
                });
            }
            await Task.CompletedTask;
        }

        public Task setPlay(int pos)
        {
            Bot.guit[pos].playing = true;
            return Task.CompletedTask;
        }

        public Task setNP(int pos, Gsets2 queue)
        {
            Bot.guit[pos].playnow.LavaTrack = queue.LavaTrack;
            Bot.guit[pos].playnow.requester = queue.requester;
            Bot.guit[pos].playnow.addtime = queue.addtime;
            return Task.CompletedTask;
        }
    }
}
