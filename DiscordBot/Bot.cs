using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace DiscordBot
{
    internal class XrhrBot
    {
        private static XrhrBot _instance;

        private DiscordClient client;

        private CommandsNextExtension commands;

        string _prefix = "!!";
        public string Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
            }
        }

        private XrhrBot()
        {
            this.SetupClient();
            this.SetupCommands();
        }

        public static XrhrBot Instance()
        {
            if (_instance == null)
            {
                _instance = new XrhrBot();
            }
            return _instance;
        }

        private void SetupClient()
        {
            if (this.client != null)
            {
                this.client.DisconnectAsync();
                this.client.Dispose();
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DISCORD_TOKEN")))
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
            if (this.commands != null)
            {
                this.commands.Dispose();
            }

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
