using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MikuMusicSharp.BotClass.BotNew;

namespace BetaPlush.Commands.MusicEx
{
    public class Functions
    {
        public Task Pause(int pos)
        {
            Bot.guit[pos].LLGuild.Pause();
            return Task.CompletedTask;
        }

        public Task Leave(int pos)
        {
            Bot.guit[pos].playing = false;
            Bot.guit[pos].rAint = 0;
            Bot.guit[pos].repeat = false;
            Bot.guit[pos].repeatAll = false;
            Bot.guit[pos].shuffle = false;
            return Task.CompletedTask;
        }

        public Task Resume(int pos)
        {
            Bot.guit[pos].LLGuild.Resume();
            return Task.CompletedTask;
        }
        public Task Repeat(int pos)
        {
            Bot.guit[pos].repeat = !Bot.guit[pos].repeat;
            return Task.CompletedTask;
        }
        public Task RepeatAll(int pos)
        {
            Bot.guit[pos].repeatAll = !Bot.guit[pos].repeatAll;
            return Task.CompletedTask;
        }
        public Task Shuffle(int pos)
        {
            Bot.guit[pos].shuffle = !Bot.guit[pos].shuffle;
            return Task.CompletedTask;
        }
        public Task Skip(int pos)
        {
            Bot.guit[pos].playing = false;
            Bot.guit[pos].LLGuild.Stop();
            return Task.CompletedTask;
        }
        public Task Stop(int pos)
        {
            Bot.guit[pos].sstop = true;
            Bot.guit[pos].playing = false;
            Bot.guit[pos].LLGuild.Stop();
            return Task.CompletedTask;
        }
    }
}
