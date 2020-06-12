using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace GameBot
{
    public class Game
    {
        public Deck deck = new Deck();
        public List<Card> pile = new List<Card>();
        public List<Player> players = new List<Player>();
        public Game() => deck.Shuffle();
    }

    public class Player
    {
        public DiscordMember user;
        public List<Card> hand;
    }

    class CrazyEightsCommand
    {
        public Game game = new Game();
        public bool b1 = false;
        public DiscordClient client = DiscordBot.discordBot;

        [Command("crazyeights")]
        [Description("The `crazy eights` command will start the game with the specified users in the `users` string.")]
        public async Task StartGame(CommandContext ctx, string users)
        {
            if (await ValidUsers(ctx, users))
                await RunGame(ctx);
        }

        [Command("place")]
        [Description("The `place` command places the given card(s) if they satisfy the conditions of Crazy Eights.")]
        public async Task Place(CommandContext ctx, string cards)
        {
            if (!ctx.Channel.IsPrivate)
            {
                await ctx.Channel.SendMessageAsync(null, false, await DiscordEmbed.ErrorMessage("Please play the game in your DMs!"));
                return;
            }

            DiscordDmChannel dm = (DiscordDmChannel)ctx.Channel;
            var player = await GetPlayer(dm.Recipients[0].Username);

            foreach (string card in cards.Split())
                if (card.Length >= 4)
                {
                    await ctx.Member.SendMessageAsync(null, false, await DiscordEmbed.ErrorMessage("Incorrect card format! Try entering the value and/or suit of one of your cards! Ex: 6 or 6D"));
                    return;
                }

            List<Card> chosen = await GetCards(ctx, player, cards);
            if (await IsPlaceable(chosen, game.pile[0]))
            {
                chosen.Reverse();
                game.pile.InsertRange(0, chosen);

                foreach (Card c in chosen)
                    player.hand.Remove(c);

                await Display.HandImg(player.hand, player.user.DisplayName);
                await Display.TopPileImg(game.pile);
                await ctx.Channel.SendMessageAsync(null, false, await DiscordEmbed.DisplayCards(await client.GetChannelAsync(721038967155458059), player));
            }
            else
                await ctx.Channel.SendMessageAsync(null, false, await DiscordEmbed.ErrorMessage("Cards cannot be placed!"));
            return;
        }

        [Command("draw")]
        [Description("The `draw` command draws one card from the deck.")]
        public async Task Draw(CommandContext ctx)
        {
            if (!ctx.Channel.IsPrivate)
            {
                await ctx.Channel.SendMessageAsync(null, false, await DiscordEmbed.ErrorMessage("Please play the game in your DMs!"));
                return;
            }

            DiscordDmChannel dm = (DiscordDmChannel)ctx.Channel;

            var player = await GetPlayer(dm.Recipients[0].Username);
            player.hand.AddRange(await game.deck.Draw(1));

            await Display.HandImg(player.hand, player.user.DisplayName);
            await ctx.Channel.SendMessageAsync(null, false, await DiscordEmbed.DisplayCards(await client.GetChannelAsync(721038967155458059), player));

            return;
        }

        public async Task RunGame(CommandContext ctx)
        {
            try
            {
                if (!b1)
                {
                    b1 = true;
                    foreach (Player player in game.players)
                    {
                        player.hand = await game.deck.Draw(5);
                        await Display.HandImg(player.hand, player.user.DisplayName);
                    }

                    game.pile.Add(game.deck.card[0]);
                    game.deck.card.RemoveAt(0);

                    await Display.TopPileImg(game.pile);
                    foreach (Player player in game.players)
                        await ctx.Member.SendMessageAsync(null, false, await DiscordEmbed.DisplayCards(await client.GetChannelAsync(721038967155458059), player));
                }
                await Task.Delay(-1);
            }
            catch(Exception e)
            {
                await ctx.Member.SendMessageAsync(e.ToString());
            }
        }

        public Task<bool> ValidUsers(CommandContext ctx, string users)
        {
            foreach (string user in users.Split())
            {
                if (new DiscordMemberConverter().TryConvert(user, ctx, out DiscordMember player))
                    game.players.Add(new Player { user = player });
                else
                    return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task<Player> GetPlayer(string username)
        {
            foreach (Player player in game.players)
                if (player.user.DisplayName == username)
                    return Task.FromResult(player);
            return null;
        }

        public Task<bool> IsPlaceable(List<Card> test, Card top)
        {
            bool check = true;
            if (test[0].value == top.value || test[0].suit == top.suit || test[0].value == "8")
            {
                for (int i = 1; i < test.Count; i++)
                    if (!(test[i].value == test[i - 1].value))
                    {
                        check = false;
                        break;
                    }
                return Task.FromResult(check);
            }
            return Task.FromResult(false);
        }

        public async Task<List<Card>> GetCards(CommandContext ctx, Player player, string s)
        {
            List<Card> cards = new List<Card>();
            foreach (string card in s.Split())
            {
                bool check = false;
                foreach (Card c in player.hand)
                {
                    if ((c.value[0] == card[0] && c.suit[0] == card[1]) || c.value == card[0].ToString() + card[1].ToString() && c.suit[0] == card[2])
                    {
                        check = true;
                        cards.Add(c);
                        break;
                    }
                }
                if (!check)
                {
                    await ctx.Member.SendMessageAsync(null, false, await DiscordEmbed.ErrorMessage("Card not found!"));
                    return new List<Card>();
                }
            }
            return cards;
        }
    }
}
