using System;
using System.Drawing;

namespace maloveevalaba
{
    class embossingFilter : MatrixFilter
    {
        private float[,] kernel = new float[,] {
         { 0, 1,  0 },
         { 1, 0, -1 },
         { 0, -1,  0 }
        };

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = 0;
            float resultG = 0;
            float resultB = 0;

            for (int l = -radiusY; l <= radiusY; l++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            }

            resultR = (resultR + 255) / 2;
            resultG = (resultG + 255) / 2;
            resultB = (resultB + 255) / 2;

            resultR = Clamp((int)resultR, 0, 255);
            resultG = Clamp((int)resultG, 0, 255);
            resultB = Clamp((int)resultB, 0, 255);

            return Color.FromArgb((int)resultR, (int)resultG, (int)resultB);
        }

        private int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
