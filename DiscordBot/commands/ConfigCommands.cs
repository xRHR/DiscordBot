using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiscordBot.Commands
{
    internal class ConfigCommands : BaseCommandModule
    {
        [Command("prefix")]
        public async Task Prefix(CommandContext ctx, string prefix)
        {
            try
            {
                var bot = XrhrBot.Instance();
                bot.Prefix = prefix;

                await ctx.RespondAsync($"Prefix set to ```{prefix}```");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Command("nickname")]
        public async Task Nickname(CommandContext ctx, string nickname)
        {
            try
            {
                if (ctx.Member is null)
                {
                    await ctx.RespondAsync("You must be in a guild to use this command.");
                    return;
                }
                await ctx.Member.ModifyAsync(x => x.Nickname = nickname);
                await ctx.Channel.SendMessageAsync($"Changed nickname to ```{nickname}```");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
