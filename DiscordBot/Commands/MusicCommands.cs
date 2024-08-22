using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    internal class MusicCommands : BaseCommandModule
    {
        [Command("play")]
        public async Task Play(CommandContext ctx, string url)
        {
            if (ctx.Member == null) return;

            var lavalinkInstance = ctx.Client.GetLavalink();

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                var emb = new DiscordEmbedBuilder
                {
                    Title = "Че за хуйня",
                    Description = "Не могу подключиться к серверу Lavalink",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync(embed: emb);
                return;
            }

            var vc = ctx.Member.VoiceState.Channel;
            if (vc == null || vc.Type != DSharpPlus.ChannelType.Voice)
            {
                var emb = new DiscordEmbedBuilder
                {
                    Title = "Ты глупый?",
                    Description = "Я куда по-твоему музыку тебе играть должен? В войс-то зайди",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync(embed: emb);
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            await node.ConnectAsync(vc);

            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                var emb = new DiscordEmbedBuilder
                {
                    Title = "Не получилось подключиться",
                    Description = "Хз короче Lavalink не хочет подключаться",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync(embed: emb);
                return;
            }

            var search_query = await node.Rest.GetTracksAsync(url);

            if (search_query.LoadResultType is LavalinkLoadResultType.LoadFailed or LavalinkLoadResultType.NoMatches)
            {
                var emb = new DiscordEmbedBuilder
                {
                    Title = "Ниче не нашел",
                    Description = "Увы",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync(embed: emb);
                return;
            }

            var music_track = search_query.Tracks.First();

            await conn.PlayAsync(music_track);

            var now_playing_emb = new DiscordEmbedBuilder
            {
                Title = "Сейчас играет",
                Description = music_track.Title + "\n" + music_track.Author + "\n" + music_track.Uri,
                Color = DiscordColor.Green
            };

            await ctx.RespondAsync(embed: now_playing_emb);
        }
    }
}
