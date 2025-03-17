using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class GlassEffectFilter : Filters
    {
        private Random random = new Random();

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int offset = 2;

            int randomX = Clamp(x + random.Next(-offset, offset + 1), 0, sourceImage.Width - 1);
            int randomY = Clamp(y + random.Next(-offset, offset + 1), 0, sourceImage.Height - 1);

            Color neighborColor = sourceImage.GetPixel(randomX, randomY);

            return neighborColor;
        }
    }
}
