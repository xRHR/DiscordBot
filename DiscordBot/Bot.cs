using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace DiscordBot
{
    internal class XrhrBot
    {
        private DiscordClient client;

        private CommandsNextExtension commands;

        string _prefix = "!!";
        public string Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                this.SetupCommands();
            }
        }

        public XrhrBot()
        {
            this.SetupClient();
            this.SetupCommands();
        }

        private void SetupClient()
        {
            if (Environment.GetEnvironmentVariable("DISCORD_TOKEN") == null)
            {
                throw new Exception("Environment variable DISCORD_TOKEN is not set");
            }
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

        private void SetupCommands()
        {
            var cmd_cfg = new CommandsNextConfiguration()
            {
                StringPrefixes = [Prefix],
                EnableDms = true,
                EnableMentionPrefix = true,
                EnableDefaultHelp = false
            };

            this.commands = this.client.UseCommandsNext(cmd_cfg);

            this.commands.RegisterCommands<ConfigCommands>();
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
