using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Threading.Tasks;
using System.Linq;
using DSharpPlus.CommandsNext;
using PlushMusic.BotClass.BotNew;

namespace PlushMusic.Commands.Audio
{
    public class Events
    {
        public static async Task PlayFin(TrackFinishEventArgs lg)
        {
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == lg.Player.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream && !Bot.guit[pos].playnow.sstop)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester,
                    addtime = Bot.guit[pos].playnow.addtime
                });
                Console.WriteLine("LL Stream Error");
            }
            Bot.guit[pos].playing = false;
            await Task.CompletedTask;
        }

        public static async Task PlayStu(TrackStuckEventArgs ts)
        {
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == ts.Player.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream && !Bot.guit[pos].playnow.sstop)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester,
                    addtime = Bot.guit[pos].playnow.addtime
                });
            }
            await ts.Player.Guild.GetChannel(Bot.guit[pos].cmdChannel).SendMessageAsync("Track was stuck, so it was skipped >>");
            Console.WriteLine("LL Stuck");
            Bot.guit[pos].playing = false;
            await Task.CompletedTask;
        }

        public static async Task PlayErr(TrackExceptionEventArgs ts)
        {
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == ts.Player.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream && !Bot.guit[pos].playnow.sstop)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester,
                    addtime = Bot.guit[pos].playnow.addtime
                });
            }
            await ts.Player.Guild.GetChannel(Bot.guit[pos].cmdChannel).SendMessageAsync($"There was an error with the track, so it was skipped ({ts.Error.First()})>>");
            Console.WriteLine("LL Error");
            Bot.guit[pos].playing = false;
            await Task.CompletedTask;
        }

        public static Task setPlay(int pos)
        {
            Bot.guit[pos].playing = true;
            return Task.CompletedTask;
        }

        public static async Task stuckCheck(int pos, CommandContext ctx)
        {
            await Task.Delay(5000);
            if (!Bot.guit[pos].playing && Bot.guit[pos].queue.Any())
            {
                await ctx.Guild.GetChannel(Bot.guit[pos].cmdChannel).SendMessageAsync("Seems like something got stuck uwu, restarting playback >>");
                Playback.QueueLoop(pos, ctx);
            }
            await Task.CompletedTask;
        }

        public static Task setNP(int pos, Gsets2 queue)
        {
            Bot.guit[pos].playnow.LavaTrack = queue.LavaTrack;
            Bot.guit[pos].playnow.requester = queue.requester;
            Bot.guit[pos].playnow.addtime = queue.addtime;
            Bot.guit[pos].paused = false;
            if (queue.LavaTrack.IsStream)
            {
                Bot.guit[pos].playnow.sstop = false;
            }
            else
            {
                Bot.guit[pos].playnow.sstop = true;
            }
            return Task.CompletedTask;
        }
    }
}
