using DSharpPlus.CommandsNext;
using System;
using System.Linq;
using System.Threading.Tasks;
using PlushMusic.BotClass.BotNew;

namespace PlushMusic.Commands.Audio
{
    public class Playback
    {
        public static async void PlaySong(int pos, CommandContext ctx, string song) //Queue then play
        {
            if (song == null && Bot.guit[pos].queue.Count > 0) {
                await ctx.RespondAsync("Playing preloaded playlist/resuming queue!");
                Console.WriteLine($"[{ctx.Guild.Id}] Resuming queue");
            }
            if (song != null) {
                await Task.Run(() => QueueSong(pos, ctx, song));
            }       
            QueueLoop(pos, ctx);
            await Task.CompletedTask;
        }

        public static async Task QueueSong(int pos, CommandContext ctx, string song) //Queue only
        {
            DSharpPlus.Lavalink.LavalinkTrack track;
            string pora = "Playing";
            string end = "";
            if (!song.StartsWith("https://") && !song.StartsWith("http://")) {
                var tra = await Bot.guit[0].LLinkCon.GetTracksAsync(song);
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.NoMatches || tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.LoadFailed) {
                    await ctx.RespondAsync("An error occoured while loading the song, this could be due to the song being region locked uwu");
                    await Task.CompletedTask;
                    return;
                }
                track = tra.Tracks.First();
                Console.WriteLine($"[{ctx.Guild.Id}] Added to queue: {track.Title} by {track.Author}");
            } else {
                var tra = await Bot.guit[0].LLinkCon.GetTracksAsync(new Uri(song));
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.NoMatches || tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.LoadFailed) {
                    await ctx.RespondAsync("An error occoured while loading the song, this could be due to the song being region locked uwu");
                    await Task.CompletedTask;
                    return;
                }
                track = tra.Tracks.First();
                Console.WriteLine($"[{ctx.Guild.Id}] Added to queue: {track.Title} by {track.Author}");
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.PlaylistLoaded) {
                    track = tra.Tracks.ToList()[tra.PlaylistInfo.SelectedTrack];
                }
            } if (Bot.guit[pos].queue.Count > 0) {
                pora = "Added";
                end = "to the queue!";
            }
            Bot.guit[pos].queue.Add(new Gsets2{
                LavaTrack = track,
                requester = ctx.Member,
                addtime = DateTime.Now
            });
            int yoyo = Bot.guit[pos].queue.FindIndex(x => x.LavaTrack.Uri == track.Uri && x.requester == ctx.Member);
            string uwu = Bot.guit[pos].queue[yoyo].requester.Username;
            if (Bot.guit[pos].queue[yoyo].requester.Nickname != null) uwu += $" ({Bot.guit[pos].queue[yoyo].requester.Nickname})";
            await ctx.RespondAsync($"{pora}: **{Bot.guit[pos].queue[yoyo].LavaTrack.Title}** by **{Bot.guit[pos].queue[yoyo].LavaTrack.Author}** {end}\nRequested by: {uwu}");
            await Task.CompletedTask;
        }

        public static async void QueueLoop(int pos, CommandContext ctx) //Start playback without queueing
        {
            var con = Bot.guit[0].LLinkCon;
            while (Bot.guit[pos].queue.Count != 0) {
                if (Bot.guit[pos].LLGuild == null || Bot.guit[pos].stoppin) break;
                await Task.Run(() => Events.setPlay(pos));
                Random rnd = new Random();
                int rr = 0;
                if (Bot.guit[pos].shuffle) {
                    rr = rnd.Next(0, Bot.guit[pos].queue.Count);
                } if (Bot.guit[pos].repeatAll) {
                    Bot.guit[pos].rAint++;
                    rr = Bot.guit[pos].rAint;
                    if (Bot.guit[pos].rAint == Bot.guit[pos].queue.Count) {
                        Bot.guit[pos].rAint = 0;
                        rr = 0;
                    }
                }
                if (Bot.guit[pos].queue.Count != 0 || Bot.guit[pos].queue[rr] == null)
                {
                    await Task.Run(() => Events.setNP(pos, Bot.guit[pos].queue[rr]));
                }
                Console.WriteLine($"[{ctx.Guild.Id}] Started playing: {Bot.guit[pos].playnow.LavaTrack.Title} by {Bot.guit[pos].playnow.LavaTrack.Author}");
                await LavaLinkHandOff(pos, Bot.guit[pos].playnow.LavaTrack, ctx, rr);
                if (!Bot.guit[pos].repeat && !Bot.guit[pos].repeatAll && Bot.guit[pos].LLGuild != null && !Bot.guit[pos].stoppin) {
                    try {
                        Bot.guit[pos].queue.RemoveAt(rr);
                    }
                    catch { }
                    }
            }
            await Task.CompletedTask;
        }

        public static async Task LavaLinkHandOff(int pos, DSharpPlus.Lavalink.LavalinkTrack track, CommandContext ctx, int rr)
        {
            if (Bot.guit[pos].playnow.LavaTrack.IsStream)
            {
                var naet = await Bot.guit[0].LLinkCon.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].playnow.LavaTrack = naet.Tracks.First();
            }
            var datrack2 = await Bot.guit[0].LLinkCon.GetTracksAsync(new Uri(Bot.guit[pos].queue[rr].LavaTrack.Uri.OriginalString));
            if (datrack2.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.NoMatches || datrack2.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.LoadFailed) {
                try {
                    Bot.guit[pos].queue.RemoveAt(rr);
                }
                catch { }
                await ctx.RespondAsync("Track error, maybe regionlocked, skipped >>");
                Console.WriteLine($"[{ctx.Guild.Id}] Song errored/regionblocked");
                await Task.CompletedTask;
                return;
            }
            Bot.guit[pos].LLGuild.Play(track);
            while (Bot.guit[pos].playing) {
                await Task.Delay(25);
            }
            await Task.CompletedTask;
        }
    }
}
