using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using DiscordBot;

string warn = "";
bool err = false;

string? lavalinkUrl = ConfigurationManager.AppSettings["lavalinkHostUrl"];

if (string.IsNullOrWhiteSpace(lavalinkUrl))
{
    warn += "Warning. Configuration does not contain a setting \"lavalinkHostUrl\". Trying to get environment variable LAVALINK_HOST_URL...\n";
    lavalinkUrl = Environment.GetEnvironmentVariable("LAVALINK_HOST_URL");

    if (string.IsNullOrWhiteSpace(lavalinkUrl))
    {
        warn += "Error. Environment variable \"LAVALINK_HOST_URL\" not set.\n";
        err = true;
    }
    else
    {
        lavalinkUrl = lavalinkUrl?.Trim();
        ConfigurationManager.AppSettings["lavalinkHostUrl"] = lavalinkUrl;
    }
}

string? lavalinkPassword = ConfigurationManager.AppSettings["lavalinkHostPassword"];

if (string.IsNullOrWhiteSpace(lavalinkPassword))
{
    warn += "Warning. Configuration does not contain a setting \"lavalinkHostPassword\". Trying to get environment variable LAVALINK_HOST_PASSWORD...\n";
    lavalinkPassword = Environment.GetEnvironmentVariable("LAVALINK_HOST_PASSWORD");

    if (string.IsNullOrWhiteSpace(lavalinkPassword))
    {
        warn += "Error. Environment variable \"LAVALINK_HOST_PASSWORD\" not set.\n";
        err = true;
    }
    else
    {
        lavalinkUrl = lavalinkUrl?.Trim();
        ConfigurationManager.AppSettings["lavalinkHostUrl"] = lavalinkUrl;
    }
}

if (!string.IsNullOrWhiteSpace(warn))
{
    Console.WriteLine(warn);
    if (err)
    {
        throw new Exception("Lavalink host isn't provided.");
    }
}

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
    config.BaseAddress = new Uri(lavalinkUrl ?? "");
    config.Passphrase = lavalinkPassword ?? "";
});

IHost host = builder.Build();

host.Run();