using System.Threading.Tasks;
using PlushMusic.BotClass.BotNew;

namespace PlushMusic.Commands.Audio
{
    public class Functions
    {
        public static Task Pause(int pos)
        {
            Bot.guit[pos].paused = true;
            Bot.guit[pos].LLGuild.Pause();
            return Task.CompletedTask;
        }

        public static Task Leave(int pos)
        {
            Bot.guit[pos].paused = false;
            Bot.guit[pos].playing = false;
            Bot.guit[pos].rAint = 0;
            Bot.guit[pos].repeat = false;
            Bot.guit[pos].repeatAll = false;
            Bot.guit[pos].shuffle = false;
            Bot.guit[pos].stoppin = false;
            return Task.CompletedTask;
        }

        public static Task Resume(int pos)
        {
            Bot.guit[pos].paused = false;
            Bot.guit[pos].LLGuild.Resume();
            return Task.CompletedTask;
        }
        public static Task Repeat(int pos)
        {
            Bot.guit[pos].repeat = !Bot.guit[pos].repeat;
            return Task.CompletedTask;
        }
        public static Task RepeatAll(int pos)
        {
            Bot.guit[pos].repeatAll = !Bot.guit[pos].repeatAll;
            return Task.CompletedTask;
        }
        public static Task Shuffle(int pos)
        {
            Bot.guit[pos].shuffle = !Bot.guit[pos].shuffle;
            return Task.CompletedTask;
        }
        public static Task Skip(int pos)
        {
            Bot.guit[pos].paused = false;
            Bot.guit[pos].LLGuild.Stop();
            return Task.CompletedTask;
        }
        public static Task Stop(int pos)
        {
            Bot.guit[pos].paused = false;
            Bot.guit[pos].stoppin = true;
            Bot.guit[pos].playing = false;
            Bot.guit[pos].LLGuild.Stop();
            return Task.CompletedTask;
        }
    }
}
