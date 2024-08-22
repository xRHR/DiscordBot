using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiscordBot.Commands
{
    internal class ConfigCommands : BaseCommandModule
    {
        [Command("prefix")]
        public async Task Prefix(CommandContext ctx, string prefix)
        {

            await ctx.RespondAsync($"Prefix set to ```{prefix}```");
        }

        [Command("nickname")]
        public async Task Nickname(CommandContext ctx, string nickname)
        {
            if (ctx.Member is null) return;
            await ctx.Member.ModifyAsync(x => x.Nickname = nickname);
            await ctx.Channel.SendMessageAsync($"Changed nickname to ```{nickname}```");
        }
    }
}
