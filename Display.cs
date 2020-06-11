using System.Collections.Generic;
using System.Threading.Tasks;
using ImageMagick;

namespace GameBot
{
    public class Display
    {
        public static Task HandImg(List<Card> hand)
        {
            using (var images = new MagickImageCollection())
            {
                foreach (Card card in hand)
                    images.Add(card.img);

                using (var result = images.AppendHorizontally())
                {
                    result.Write("hand.png");
                    return Task.CompletedTask;
                }
            }
        }

        public static Task TopDeckImg(Deck deck)
        {
            using (var images = new MagickImageCollection())
            {
                images.Add(deck.color);
                images.Add(deck.card[0].img);

                using (var result = images.AppendHorizontally())
                {
                    result.Write("top.png");
                    return Task.CompletedTask;
                }
            }
        }
    }
}
