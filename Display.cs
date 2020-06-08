using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
