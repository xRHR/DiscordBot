using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace DiscordBot
{

    internal sealed class DiscordClientHost : IHostedService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _serviceProvider;

        public DiscordClientHost(
            DiscordSocketClient discordSocketClient,
            InteractionService interactionService,
            IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(discordSocketClient);
            ArgumentNullException.ThrowIfNull(interactionService);
            ArgumentNullException.ThrowIfNull(serviceProvider);

            _discordSocketClient = discordSocketClient;
            _interactionService = interactionService;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _discordSocketClient.InteractionCreated += InteractionCreated;
            _discordSocketClient.Ready += ClientReady;

            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new NullReferenceException("Environment variable DISCORD_TOKEN is not set");
            }

            // Put bot token here
            await _discordSocketClient
                .LoginAsync(TokenType.Bot, token)
                .ConfigureAwait(false);

            await _discordSocketClient
                .StartAsync()
                .ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _discordSocketClient.InteractionCreated -= InteractionCreated;
            _discordSocketClient.Ready -= ClientReady;

            await _discordSocketClient
                .StopAsync()
                .ConfigureAwait(false);
        }

        private Task InteractionCreated(SocketInteraction interaction)
        {
            var interactionContext = new SocketInteractionContext(_discordSocketClient, interaction);
            return _interactionService!.ExecuteCommandAsync(interactionContext, _serviceProvider);
        }

        private async Task ClientReady()
        {
            await _interactionService
                .AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider)
                .ConfigureAwait(false);

            // Put your guild id to test here
            await _interactionService
                .RegisterCommandsToGuildAsync(287458474206691328)
                .ConfigureAwait(false);
        }
    }
}
