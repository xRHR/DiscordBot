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

        [Command("me")]
        public async Task Prefix(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync(ctx.Client.CurrentUser.Username);
        }
    }
}
