﻿using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Extensions;
using Lavalink4NET.InactivityTracking;
using Lavalink4NET.Players;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using DiscordBot;
using Lavalink4NET.InactivityTracking.Trackers.Idle;
using Lavalink4NET.InactivityTracking.Trackers.Users;
using Lavalink4NET.InactivityTracking.Extensions;
using System.Collections.Immutable;

string configFilePath = Path.Combine(AppContext.BaseDirectory, "lavalink-host.json");
IConfiguration lavalink_cfg = new ConfigurationBuilder()
    .AddJsonFile(configFilePath, optional: false, reloadOnChange: true)
    .Build();

string err = "";
string? lavalinkUrl = lavalink_cfg.GetValue<string>("Url");

if (string.IsNullOrWhiteSpace(lavalinkUrl))
{
    err += "Error. Lavalink URL is not set.\n";
}

string? lavalinkPassword = lavalink_cfg.GetValue<string>("Password");

if (string.IsNullOrWhiteSpace(lavalinkPassword))
{
    err += "Error. Lavalink password is not set.\n";
}

if (!string.IsNullOrWhiteSpace(err))
{
    Console.WriteLine(err);
    throw new Exception("Lavalink host isn't provided.");
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

builder.Services.AddInactivityTracking();

builder.Services.Configure<IdleInactivityTrackerOptions>(config =>
{
    config.Timeout = TimeSpan.FromSeconds(10);
});
builder.Services.Configure<UsersInactivityTrackerOptions>(config =>
{
    config.Timeout = TimeSpan.FromSeconds(10);
});

IHost host = builder.Build();

host.Run();