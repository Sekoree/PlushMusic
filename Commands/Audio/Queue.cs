using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlushMusic.BotClass.BotNew;

namespace PlushMusic.Commands.Audio
{
    public class Queue
    {
        public static async Task QueueList(int pos,CommandContext ctx)
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
                        emboi.AddField($"{upboi}.{item.LavaTrack.Title} ({time2})", $"By **{item.LavaTrack.Author}** [Link]({item.LavaTrack.Uri}) **||** Requested by {item.requester.Mention}\nᵕ");
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

        public static Task queueRemove(int pos, int num)
        {
            Bot.guit[pos].queue.RemoveAt(num);
            return Task.CompletedTask;
        }

        public static Task queueRemoveSome(int pos, int num, int maxVal)
        {
            Bot.guit[pos].queue.RemoveRange(num, maxVal);
            return Task.CompletedTask;
        }
    }
}
