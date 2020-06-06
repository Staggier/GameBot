using System.Threading.Tasks;
using DSharpPlus.Entities;

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
    }
}
