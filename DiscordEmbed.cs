using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;

namespace GameBot
{
    public class DiscordEmbed
    {
        public static Task<DiscordEmbedBuilder> HelpMessage(string cmd, string desc)
        {
            return Task.FromResult(new DiscordEmbedBuilder
            {
                Color = new DiscordColor(66, 245, 173),
                Description = desc,
                Title = cmd != "" ? "Command" : "Commands",
            });
        }

        public static Task<DiscordEmbedBuilder> ErrorMessage(string desc)
        {
            return Task.FromResult(new DiscordEmbedBuilder
            {
                Color = new DiscordColor(209, 6, 6),
                Description = desc,
                Title = "Error:"
            });
        }

        public static async Task<DiscordEmbedBuilder> DisplayCards(DiscordChannel channel, Player player)
        {
            return new DiscordEmbedBuilder
            { 
                ThumbnailUrl = (await channel.SendFileAsync("top.png")).Attachments[0].Url,
                ImageUrl = (await channel.SendFileAsync($"{player.user.DisplayName}_hand.png")).Attachments[0].Url,
                Title = "Your hand and the last card played."
            };
        }
    }
}