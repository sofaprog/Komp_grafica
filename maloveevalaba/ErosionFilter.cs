using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class ErosionFilter : MorphologicalFilter
    {
        public ErosionFilter(int size) : base(size) { }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int halfKernelSize = kernelSize / 2;
            int minValue = 255;

            for (int i = -halfKernelSize; i <= halfKernelSize; i++)
            {
                for (int j = -halfKernelSize; j <= halfKernelSize; j++)
                {
                    int idX = Clamp(x + i, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + j, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    int gray = (int)(0.299 * neighborColor.R + 0.587 * neighborColor.G + 0.114 * neighborColor.B);
                    minValue = Math.Min(minValue, gray);
                }
            }
            return Color.FromArgb(minValue, minValue, minValue);
        }
    }
}
