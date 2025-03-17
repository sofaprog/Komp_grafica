using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class GradientFilter : Filters
    {
        private float[,] kernelX = new float[,] {
        {-1, 0, 1},
        {-2, 0, 2},
        {-1, 0, 1}
    };

        private float[,] kernelY = new float[,] {
        {-1, -2, -1},
        { 0,  0,  0},
        { 1,  2,  1}
    };

        // Переопределение метода для вычисления нового цвета пикселя
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernelX.GetLength(0) / 2;
            int radiusY = kernelY.GetLength(1) / 2;

            float gradX = 0;
            float gradY = 0;

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    int idX = Clamp(x + j, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + i, 0, sourceImage.Height - 1);

                    Color neighborColor = sourceImage.GetPixel(idX, idY);

                    // Вычисление градиента по оси X
                    gradX += neighborColor.R * kernelX[i + radiusY, j + radiusX];
                    gradX += neighborColor.G * kernelX[i + radiusY, j + radiusX];
                    gradX += neighborColor.B * kernelX[i + radiusY, j + radiusX];

                    // Вычисление градиента по оси Y
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
