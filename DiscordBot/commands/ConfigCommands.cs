using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace DiscordBot.Commands
{
    internal class ConfigCommands : BaseCommandModule
    {
        [SlashCommand("nickname", description: "Change bot nickname")]
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
