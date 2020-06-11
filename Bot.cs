using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;

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

            commands = discordBot.UseCommandsNext(new CommandsNextConfiguration { StringPrefix = "!", EnableDefaultHelp = false });

            commands.RegisterCommands<HelpCommand>();
            commands.RegisterCommands<CrazyEightsCommand>();

            await discordBot.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}