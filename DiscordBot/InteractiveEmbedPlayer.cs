using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot
{
    internal class InteractiveEmbedPlayer
    {
        public async Task PlayAsync(IVoiceChannel voiceChannel)
        {
            var emb = new EmbedBuilder
            {
                Title = "Interactive Music Player",
                Color = Color.Red
            };
        }
    }
}
