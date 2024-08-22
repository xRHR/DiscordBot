using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    internal class Bot
    {
        private DiscordClient client;

        private CommandsNextExtension commands;

        string _prefix = "!!";
        public string Prefix { get { return _prefix; } }
        //Environment.GetEnvironmentVariable("TG_TOKEN");

        public Bot()
        {
            var cfg = new DiscordConfiguration
            {
                Intents = DiscordIntents.All,
                Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };
            this.client = new DiscordClient(cfg);
            this.client.Ready += ClientReady;
        }

        public async Task Run()
        {
            await this.client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task ClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
