using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class SobelFilter : MatrixFilter
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

                    // Для фильтра Собеля нужно вычислять градиент яркости.
                    // Используем только один канал (например, яркость).
                    int grayValue = (int)(0.2989 * neighborColor.R + 0.5870 * neighborColor.G + 0.1140 * neighborColor.B);

                    // Применение ядра Собеля для оси X
                    gradX += grayValue * kernelX[i + radiusY, j + radiusX];

                    // Применение ядра Собеля для оси Y
                    gradY += grayValue * kernelY[i + radiusY, j + radiusX];
                }
            }

            // Вычисляем величину градиента
            int gradient = (int)Math.Sqrt(gradX * gradX + gradY * gradY);

            // Ограничиваем значение градиента в пределах [0, 255]
            gradient = Clamp(gradient, 0, 255);

            return Color.FromArgb(gradient, gradient, gradient);
        }
    }

}
