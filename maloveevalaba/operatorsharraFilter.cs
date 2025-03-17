using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class operatorsharraFilter : MatrixFilter
    {
        private float[,] kernelY = new float[,] {
        {3, 10, 3},
        {0, 0, 0},
        {-3, -10, -3}
    };

        private float[,] kernelX = new float[,] {
        {3, 0, -3},
        { 10,  0,  -10},
        { 3,  0,  -3}
    };
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernelX.GetLength(0) / 2;
            int radiusY = kernelX.GetLength(1) / 2;

            float gradX = 0;
            float gradY = 0;

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    int idX = Clamp(x + j, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + i, 0, sourceImage.Height - 1);

                    Color neighborColor = sourceImage.GetPixel(idX, idY);

                    gradX += neighborColor.R * kernelX[i + radiusY, j + radiusX];
                    gradX += neighborColor.G * kernelX[i + radiusY, j + radiusX];
                    gradX += neighborColor.B * kernelX[i + radiusY, j + radiusX];

                    gradY += neighborColor.R * kernelY[i + radiusY, j + radiusX];
                    gradY += neighborColor.G * kernelY[i + radiusY, j + radiusX];
                    gradY += neighborColor.B * kernelY[i + radiusY, j + radiusX];
                }
            }

            int gradient = (int)Math.Sqrt(gradX * gradX + gradY * gradY);

            gradient = Clamp(gradient, 0, 255);

            return Color.FromArgb(gradient, gradient, gradient);
        }

    }
}
