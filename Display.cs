using System.Collections.Generic;
using System.Threading.Tasks;
using ImageMagick;

namespace GameBot
{
    public class Display
    {
        public static Task HandImg(List<Card> hand)
        {
            using (var image = new MagickImage("xc:none", 100 * hand.Count - 1 + 400, 726))
            {
                int x = 0, y = 0;
                foreach (Card card in hand)
                {
                    image.Composite(new MagickImage(card.img), x, y, CompositeOperator.Over);
                    x += 100;
                }
                image.Write("hand.png");
                return Task.CompletedTask;
            }
        }

        public static Task TopPileImg(List<Card> pile)
        {
            using (var image = new MagickImage("xc:none", 500, 726))
            {
                image.Composite(new MagickImage(pile[0].img), 0, 0, CompositeOperator.Over);
                image.Write("top.png");
                return Task.CompletedTask;
            }
        }
    }
}
