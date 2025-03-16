using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class SepiaFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int k = 30;
            int r = sourceColor.R+2*k;
            int g = (int)(sourceColor.G+(0.5*k));
            int b = sourceColor.B-1*k;

            r = Clamp(r, 0, 255);
            g = Clamp(g, 0, 255);
            b = Clamp(b, 0, 255);

            return Color.FromArgb(r, g, b);
        }
    }
}
