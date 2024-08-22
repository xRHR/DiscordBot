using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

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
                Task<DiscordMember> get_self_member = ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
                await get_self_member;
                await get_self_member.Result.ModifyAsync(x => x.Nickname = nickname);
                await ctx.Channel.SendMessageAsync($"Changed nickname to ```{nickname}```");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
