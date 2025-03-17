using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class GrayWorldFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);

            // Вычисление среднего значения каналов R, G, B
            int average = (sourceColor.R + sourceColor.G + sourceColor.B) / 3;

            return Color.FromArgb(average, average, average);
        }
    }
}
