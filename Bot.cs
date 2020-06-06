using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Net;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Collections.Immutable;

namespace GameBot
{
    class DiscordBot
    {
        public static DiscordClient discordBot;
        public static CommandsNextModule commands;
        public static string token = new StreamReader("token.txt").ReadLine();

        public async Task MainAsync(string[] args)
        {
            discordBot = new DiscordClient(new DiscordConfiguration
            {
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug,
                TokenType = TokenType.Bot,
                Token = token
            });

            commands = discordBot.UseCommandsNext(new CommandsNextConfiguration { StringPrefix = "!" });

            await discordBot.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}