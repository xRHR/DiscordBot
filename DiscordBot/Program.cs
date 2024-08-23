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
builder.Services.AddSingleton<InteractionService>();
builder.Services.AddHostedService<DiscordClientHost>();

// Lavalink
builder.Services.AddLavalink();
builder.Services.AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace));

IHost host = builder.Build();
    
host.Run();