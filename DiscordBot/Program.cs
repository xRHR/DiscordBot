using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DiscordBot;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Discord
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
builder.Services.AddHostedService<DiscordClientHost>();

// Lavalink
builder.Services.AddLavalink();
builder.Services.AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace));
builder.Services.ConfigureLavalink(config =>
{
    config.BaseAddress = new Uri("https://v4.lavalink.rocks:443/");
    config.Passphrase = "horizxon.tech";
});

IHost host = builder.Build();

host.Run();