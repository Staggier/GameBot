using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ImageMagick;

namespace GameBot
{
    public class Card
    {
        public MagickImage cardImg;
        public string suit, value;

        public Card(string card)
        {
            int len = card.Length;
            cardImg = new MagickImage($"Deck/{card}.png");
            value = len == 2 ? card[0].ToString() : card[0].ToString() + card[1].ToString();
            suit = len == 2 ? card[1].ToString() : card[2].ToString();
        }
    }

    public class Deck
    {
        public List<Card> deck;
        public Deck() => deck = CreateDeck().Result;

        public async Task<List<Card>> CreateDeck()
        {
            string[] cards = new string[]
            {
                 "AC", "AD", "AH", "AS", "2C", "2D", "2H", "2S", "3C", "3D",  "3H",  "3S",  "4C",
                 "4D", "4H", "4S", "5C", "5D", "5H", "5S", "6C", "6D", "6H",  "6S",  "7C",  "7D",
                 "7H", "7S", "8C", "8D", "8H", "8S", "9C", "9D", "9H", "9S", "10C", "10D", "10H",
                "10S", "JC", "JD", "JH", "JS", "QC", "QD", "QH", "QS", "KC",  "KD",  "KH",  "KS"
            };

            List<Card> deck = new List<Card>();
            foreach(string s in cards)
                deck.Add(new Card(s));

            return await Task.FromResult(deck);
        }

        public Task Shuffle()
        {
            int n = deck.Count;
            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                int j = random.Next(n);
                Card c = deck[j];

                deck[j] = deck[i];
                deck[i] = c;
            }
            return Task.CompletedTask;
        }
    }
}