using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class WavesFilter2 : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int newX = Clamp(x + (int)(20 * Math.Sin(2 * Math.PI * y / 60)), 0, sourceImage.Width - 1);
            int newY = y;
            return sourceImage.GetPixel(newX, newY);
        }
    }
}
