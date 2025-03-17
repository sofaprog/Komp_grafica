using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class MedianFilter : Filters
    {
        private int radius;

        //размеры окна
        public MedianFilter(int radius = 1)
        {
            this.radius = radius;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            List<int> rValues = new List<int>();
            List<int> gValues = new List<int>();
            List<int> bValues = new List<int>();

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    int newX = Clamp(x + i, 0, sourceImage.Width - 1);
                    int newY = Clamp(y + j, 0, sourceImage.Height - 1);
                    Color color = sourceImage.GetPixel(newX, newY);

                    rValues.Add(color.R);
                    gValues.Add(color.G);
                    bValues.Add(color.B);
                }
            }

            rValues.Sort();
            gValues.Sort();
            bValues.Sort();

            int medianIndex = rValues.Count / 2;

            return Color.FromArgb(rValues[medianIndex], gValues[medianIndex], bValues[medianIndex]);
        }
    }
}
