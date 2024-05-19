using Discord;
using Discord.Webhook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Manager
{
    public class NotificationManager : Singleton<NotificationManager>
    {
        public NotificationManager() { }

        public async Task PublishDiscordWebHook(Exception? ex, object obj, string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                using (var discordClient = new DiscordWebhookClient(url))
                {
                    var embed = new EmbedBuilder { Title = "Test Embed", Description = "Test Description" };
                    await discordClient.SendMessageAsync(text: "Send a message to this webhook!", embeds: new[] { embed.Build() });
                }
            }
        }
        public async Task PublishRedis(string message, Exception? ex, object obj, string url, string channel)
        {

        }
        //public async Task PublishElasticSearch(object obj, string url)
        //{

        //}

        //public bool TryInitElasticSearch(string url, string login, string password, string index)
        //{
        //    return true;
        //}
    }
}
