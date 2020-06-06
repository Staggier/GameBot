namespace GameBot
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordBot bot = new DiscordBot();
            bot.MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}