using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using MikuMusicSharp.BotClass.BotNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlushMusic.Program;

namespace BetaPlush.Commands.MusicEx
{
    public class Queue
    {
        public async Task QueueList(int pos, CommandContext ctx)
        {
            try
            {
                var intbi = ctx.Client.GetInteractivity();
                var chn = ctx.Member.VoiceState?.Channel;
                if (Bot.guit[pos].LLGuild.Channel != chn)
                {
                    await Task.CompletedTask;
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (Bot.guit[pos].queue.Count == 0 && !Bot.guit[pos].playing)
                {
                    await ctx.RespondAsync("Queue is empty uwu");
                    return;
                }
                else if (Bot.guit[pos].queue.Count == 0 && Bot.guit[pos].playing)
                {
                    var eb = new DiscordEmbedBuilder
                    {
                        Color = new DiscordColor("#68D3D2"),
                        Title = "Current Queue",
                        Description = "more",
                        ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                    };
                    var que = Bot.guit[pos].queue;
                    eb.WithDescription("**__Now Playing:__**");
                    string time1 = "";
                    string time2 = "";
                    if (Bot.guit[pos].playnow.LavaTrack.Length.Hours < 1)
                    {
                        time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                        time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"mm\:ss");
                    }
                    else
                    {
                        time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                        time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                    }
                    eb.AddField($"{Bot.guit[pos].playnow.LavaTrack.Title} ({time1}/{time2})", $"By **{Bot.guit[pos].playnow.LavaTrack.Author}** [Link]({Bot.guit[pos].playnow.LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].playnow.requester.Mention}\n-----");
                    await ctx.RespondAsync(embed: eb.Build());
                    return;
                }
                List<Page> dapa = new List<Page>();
                var emboi = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#68D3D2"),
                    Title = "Current Queue",
                    Description = "more",
                    ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                };
                int up = 1;
                int upboi = 1;
                if (Bot.guit[pos].repeatAll)
                {
                    int rAindex = Bot.guit[pos].queue.FindIndex(x => x.addtime == Bot.guit[pos].playnow.addtime);
                    if (rAindex == -1) return;
                    foreach (var item in Bot.guit[pos].queue)
                    {
                        if (item.addtime == Bot.guit[pos].playnow.addtime)
                        {
                            emboi.WithDescription("**__Now Playing:__**");
                            string time1 = "";
                            string time2 = "";
                            if (Bot.guit[pos].playnow.requester == null)
                            {
                                if (Bot.guit[pos].queue.First().LavaTrack.Length.Hours < 1)
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                                    time2 = Bot.guit[pos].queue[rAindex].LavaTrack.Length.ToString(@"mm\:ss");
                                }
                                else
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                                    time2 = Bot.guit[pos].queue[rAindex].LavaTrack.Length.ToString(@"hh\:mm\:ss");
                                }
                                emboi.AddField($"**{rAindex}**.{Bot.guit[pos].queue[rAindex].LavaTrack.Title} ({time1}/{time2})", $"By **{Bot.guit[pos].queue[rAindex].LavaTrack.Author}** [Link]({Bot.guit[pos].queue[rAindex].LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].queue[rAindex].requester.Mention}\n-----");
                                rAindex++;
                            }
                            else
                            {
                                if (Bot.guit[pos].playnow.LavaTrack.Length.Hours < 1)
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                                    time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"mm\:ss");
                                }
                                else
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                                    time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                                }
                                emboi.AddField($"**{rAindex}**.{Bot.guit[pos].playnow.LavaTrack.Title} ({time1}/{time2})", $"By **{Bot.guit[pos].playnow.LavaTrack.Author}** [Link]({Bot.guit[pos].playnow.LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].playnow.requester.Mention}\n-----");
                                rAindex++;
                            }
                        }
                        else
                        {
                            string time2 = "";

                            if (item.LavaTrack.Length.Hours < 1)
                            {
                                time2 = Bot.guit[pos].queue[rAindex].LavaTrack.Length.ToString(@"mm\:ss");
                            }
                            else
                            {
                                time2 = Bot.guit[pos].queue[rAindex].LavaTrack.Length.ToString(@"hh\:mm\:ss");
                            }
                            emboi.AddField($"**{rAindex}**.{Bot.guit[pos].queue[rAindex].LavaTrack.Title} ({time2})", $"By **{Bot.guit[pos].queue[rAindex].LavaTrack.Author}** [Link]({Bot.guit[pos].queue[rAindex].LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].queue[rAindex].requester.Mention}\nᵕ");
                            upboi++;
                            rAindex++;
                        }
                        up++;
                        if (up == 6)
                        {
                            up = 1;
                            dapa.Add(new Page
                            {
                                Embed = emboi.Build()
                            });
                            emboi = new DiscordEmbedBuilder
                            {
                                Color = new DiscordColor("#68D3D2"),
                                Title = "Current Queue",
                                Description = "more",
                                ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                            };
                        }
                        if (rAindex == Bot.guit[pos].queue.Count)
                        {
                            rAindex = 0;
                        }
                    }
                }
                else
                {
                    foreach (var item in Bot.guit[pos].queue)
                    {
                        if (item == Bot.guit[pos].queue.First())
                        {
                            emboi.WithDescription("**__Now Playing:__**");
                            string time1 = "";
                            string time2 = "";
                            if (Bot.guit[pos].playnow.requester == null)
                            {
                                if (Bot.guit[pos].queue.First().LavaTrack.Length.Hours < 1)
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                                    time2 = Bot.guit[pos].queue.First().LavaTrack.Length.ToString(@"mm\:ss");
                                }
                                else
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                                    time2 = Bot.guit[pos].queue.First().LavaTrack.Length.ToString(@"hh\:mm\:ss");
                                }
                                emboi.AddField($"{Bot.guit[pos].queue.First().LavaTrack.Title} ({time1}/{time2})", $"By **{Bot.guit[pos].queue.First().LavaTrack.Author}** [Link]({Bot.guit[pos].queue.First().LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].queue.First().requester.Mention}\n-----");
                            }
                            else
                            {
                                if (Bot.guit[pos].playnow.LavaTrack.Length.Hours < 1)
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                                    time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"mm\:ss");
                                }
                                else
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                                    time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                                }
                                emboi.AddField($"{Bot.guit[pos].playnow.LavaTrack.Title} ({time1}/{time2})", $"By **{Bot.guit[pos].playnow.LavaTrack.Author}** [Link]({Bot.guit[pos].playnow.LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].playnow.requester.Mention}\n-----");
                            }
                        }
                        else
                        {
                            string time2 = "";

                            if (item.LavaTrack.Length.Hours < 1)
                            {
                                time2 = item.LavaTrack.Length.ToString(@"mm\:ss");
                            }
                            else
                            {
                                time2 = item.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                            }
                            emboi.AddField($"**{upboi}**.{item.LavaTrack.Title} ({time2})", $"By **{item.LavaTrack.Author}** [Link]({item.LavaTrack.Uri}) **||** Requested by {item.requester.Mention}\nᵕ");
                            upboi++;
                        }
                        up++;
                        if (up == 6)
                        {
                            up = 1;
                            dapa.Add(new Page
                            {
                                Embed = emboi.Build()
                            });
                            emboi = new DiscordEmbedBuilder
                            {
                                Color = new DiscordColor("#68D3D2"),
                                Title = "Current Queue",
                                Description = "more",
                                ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                            };
                        }
                    }
                }
                if (dapa.Count > 1)
                {
                    await intbi.SendPaginatedMessage(ctx.Channel, ctx.Member, dapa, timeoutoverride: TimeSpan.FromMinutes(2));
                }
                else if (upboi < 6)
                {
                    await ctx.RespondAsync(embed: emboi.Build());
                }
                else if (dapa.Count == 0)
                {
                    await ctx.RespondAsync("Queue is empty uwu");
                }
                else
                {
                    await ctx.RespondAsync(embed: dapa[0].Embed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            await Task.CompletedTask;
        }

        public Task queueRemove(int pos, int num)
        {
            Bot.guit[pos].queue.RemoveAt(num);
            return Task.CompletedTask;
        }

        public Task queueRemoveSome(int pos, int num, int maxVal)
        {
            Bot.guit[pos].queue.RemoveRange(num, maxVal);
            return Task.CompletedTask;
        }

        public async Task QueueSong(int pos, CommandContext ctx, string song = null) //Queue only
        {
            DSharpPlus.Lavalink.LavalinkTrack track;
            var inter = ctx.Client.GetInteractivity();
            string pora = "Playing";
            string end = "";
            if (!song.StartsWith("https://") && !song.StartsWith("http://"))
            {
                var tra = await Bot.guit[0].LLinkCon.GetTracksAsync(song);
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.NoMatches || tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.LoadFailed)
                {
                    await ctx.RespondAsync("An error occoured while loading the song, this could be due to the song being region locked uwu");
                    await Task.CompletedTask;
                    return;
                }
                if (tra.Tracks.Count() == 1)
                {
                    track = tra.Tracks.First();
                    Console.WriteLine($"[{ctx.Guild.Id}] Added to queue: {track.Title} by {track.Author}");
                }
                else
                {
                    var selem = new DiscordEmbedBuilder
                    {
                        Color = new DiscordColor("#68D3D2"),
                        Title = "Results are in!",
                        Description = "Multiple tracks were found, select one!\n React with the number you want to add",
                        ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                    };
                    DiscordEmoji[] nums = { DiscordEmoji.FromName(ctx.Client, ":one:"), DiscordEmoji.FromName(ctx.Client, ":two:"), DiscordEmoji.FromName(ctx.Client, ":three:"), DiscordEmoji.FromName(ctx.Client, ":four:"), DiscordEmoji.FromName(ctx.Client, ":five:") };
                    int er = 0;
                    var TraList = tra.Tracks.ToList();
                    foreach (var ztacks in TraList)
                    {
                        if (er == tra.Tracks.ToList().Count || er == 4) break;
                        string time2 = "";
                        if (ztacks.Length.Hours < 1) time2 = ztacks.Length.ToString(@"mm\:ss");
                        else time2 = ztacks.Length.ToString(@"hh\:mm\:ss");
                        selem.AddField($"{nums[er]} **{ztacks.Title}** by **{ztacks.Author}**", $"Length: ({time2}) [Link]({ztacks.Uri.OriginalString})");
                        er++;
                    }
                    if (er == 0)
                    {
                        await ctx.RespondAsync("An error occoured while loading the song, this could be due to the song being region locked uwu");
                        await Task.CompletedTask;
                        return;
                    }
                    try
                    {
                        var mes = await ctx.RespondAsync(embed: selem.Build());
                        int o = 0;
                        foreach (var fld in mes.Embeds.First().Fields)
                        {
                            try { await mes.CreateReactionAsync(nums[o]); } catch { }
                            o++;
                        }
                        try { await mes.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":x:")); } catch { }
                        var ems = await inter.WaitForMessageReactionAsync(xe => (xe == nums[0] || xe == nums[1] || xe == nums[2] || xe == nums[3] || xe == nums[4] || xe == DiscordEmoji.FromName(ctx.Client, ":x:")), mes, ctx.User, TimeSpan.FromSeconds(30));
                        if (ems.Emoji == nums[0]) track = tra.Tracks.ElementAt(0);
                        else if (ems.Emoji == nums[1]) track = tra.Tracks.ElementAt(1);
                        else if (ems.Emoji == nums[2]) track = tra.Tracks.ElementAt(2);
                        else if (ems.Emoji == nums[3]) track = tra.Tracks.ElementAt(3);
                        else if (ems.Emoji == nums[4]) track = tra.Tracks.ElementAt(4);
                        else if (ems.Emoji == DiscordEmoji.FromName(ctx.Client, ":x:"))
                        {
                            await mes.DeleteAsync();
                            await Task.CompletedTask;
                            return;
                        }
                        else track = tra.Tracks.First();
                        await mes.DeleteAsync();
                    }
                    catch { track = tra.Tracks.First(); }
                    Console.WriteLine($"[{ctx.Guild.Id}] Added to queue: {track.Title} by {track.Author}");
                }
            }
            else
            {
                var tra = await Bot.guit[0].LLinkCon.GetTracksAsync(new Uri(song));
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.NoMatches || tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.LoadFailed)
                {
                    await ctx.RespondAsync("An error occoured while loading the song, this could be due to the song being region locked uwu");
                    await Task.CompletedTask;
                    return;
                }
                if (tra.PlaylistInfo.SelectedTrack == -1)
                {
                    await ctx.RespondAsync($"To load a playlist use ``{config.Prefix}pp (link)`` and then ``{config.Prefix}p`` to start playback (if nothing is playing at the moment)");
                    await Task.CompletedTask;
                    return;
                }
                track = tra.Tracks.First();
                Console.WriteLine($"[{ctx.Guild.Id}] Added to queue: {track.Title} by {track.Author}");
                Console.WriteLine(tra.PlaylistInfo.SelectedTrack + " yeet");
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.PlaylistLoaded) track = tra.Tracks.ElementAt(tra.PlaylistInfo.SelectedTrack);
            }
            if (Bot.guit[pos].queue.Count > 0)
            {
                pora = "Added";
                end = "to the queue!";
            }
            Bot.guit[pos].queue.Add(new Gsets2
            {
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
    }
}