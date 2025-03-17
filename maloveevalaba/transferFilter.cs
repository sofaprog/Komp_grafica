using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class transferFilter : Filters
    {
        private Random random = new Random();

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int offset = 2;

            int randomX = Clamp(x+50, 0, sourceImage.Width - 1);
            int randomY = Clamp(y+10, 0, sourceImage.Height - 1);

            Color neighborColor = sourceImage.GetPixel(randomX, randomY);

            return neighborColor;
        }
    }
}
