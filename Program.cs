using System;
using System.IO;
using MikuMusicSharp.BotClass.BotNew;
using Newtonsoft.Json;

namespace PlushMusic
{
    class Program
    {
        public static Data config;

        static void Main()
        {

            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                config = JsonConvert.DeserializeObject<Data>(json);
            }
            if (config.Token.Contains("Your Bot Token") || config.Token=="" || config.LavaLinkIP.Contains("LavaLink Server IP or Hostname") || config.LavaLinkIP == "" || config.LavaLinkPassword == "")
            {
                Console.WriteLine("no valid Token or LavaLink IP provided!");
                return;
            }

            using (var b = new Bot())
            {
                b.RunAsync().Wait();
            }
        }

        public class Data
        {
            [JsonProperty("Token")]
            public string Token { get; set; }
            [JsonProperty("Prefix")]
            public string Prefix { get; set; }
            [JsonProperty("YoutubeAPIToken")]
            public string YouTubeAPIToken { get; set; }
            [JsonProperty("LavaLinkIP")]
            public string LavaLinkIP { get; set; }
            [JsonProperty("LavaLinkPassword")]
            public string LavaLinkPassword { get; set; }
            [JsonProperty("SocketPort")]
            public int SocketPort { get; set; }
            [JsonProperty("RestPort")]
            public int RestPort { get; set; }
        }
    }
}
