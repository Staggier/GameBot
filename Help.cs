using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace GameBot
{
    class HelpCommand
    {
        [Command("help")]
        [Description("The `help` command will provide a description for a given `command` string, if empty it will provide a description for all commands.")]
        public async Task Help(CommandContext ctx, string cmd = "")
        {
            DiscordMember user = ctx.Member;
            CommandsNextModule commands = ctx.CommandsNext;

            if (commands.RegisteredCommands.ContainsKey(cmd))
            {
                DiscordEmbedBuilder embed = await HelpMessage(cmd, commands.RegisteredCommands[cmd].Description);
                await ctx.Channel.SendMessageAsync(null, false, embed);
            }
            else
            {
                string descriptions = await GetDescriptions(commands);
                DiscordEmbedBuilder embed = cmd == "" ? await HelpMessage(cmd, descriptions) : await ErrorMessage("Command not found!");

                if (cmd == "")
                    await ctx.Channel.SendMessageAsync(null, false, embed);
                else
                    await ctx.Channel.SendMessageAsync(null, false, embed);
            }
        }

        public Task<String> GetDescriptions(CommandsNextModule commands)
        {
            string desc = "";
            foreach (KeyValuePair<string, Command> command in commands.RegisteredCommands)
                desc += $"{command.Value.Description}\n";
            return Task.FromResult(desc);
        }

        public Task<DiscordEmbedBuilder> HelpMessage(string cmd, string desc)
        {
            return Task.FromResult(new DiscordEmbedBuilder
            {
                Color = new DiscordColor(66, 245, 173),
                Description = desc,
                Title = cmd != "" ? "Command" : "Commands",
            });
        }

        public Task<DiscordEmbedBuilder> ErrorMessage(string desc)
        {
            return Task.FromResult(new DiscordEmbedBuilder
            {
                Color = new DiscordColor(209, 6, 6),
                Description = desc,
                Title = "Error:"
            });
        }
    }
}