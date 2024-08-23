using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using DiscordBot;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Discord
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
builder.Services.AddHostedService<DiscordClientHost>();

string? lavalinkUrl = ConfigurationManager.AppSettings["lavalinkHostUrl"];

if (lavalinkUrl is null)
{
    Console.Write("Enter lavalink host url: ");
    lavalinkUrl = Console.ReadLine();
    ConfigurationManager.AppSettings.Set("lavalinkHostUrl", lavalinkUrl);
}

lavalinkUrl = lavalinkUrl?.Trim();

string? lavalinkPassword = ConfigurationManager.AppSettings["lavalinkHostPassword"];

if (lavalinkPassword is null)
{
    Console.Write("Enter lavalink host password: ");
    lavalinkPassword = Console.ReadLine();
    ConfigurationManager.AppSettings.Set("lavalinkHostPassword", lavalinkPassword);
}

// Lavalink
builder.Services.AddLavalink();
builder.Services.AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace));
builder.Services.ConfigureLavalink(config =>
{
    config.BaseAddress = new Uri(lavalinkUrl ?? "");
    config.Passphrase = lavalinkPassword ?? "";
});

IHost host = builder.Build();

host.Run();