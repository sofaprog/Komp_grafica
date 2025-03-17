using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class RotateFilter : MatrixFilter
    {
        private int x0, y0; // Центр поворота
        private float angle; // Угол поворота в радианах

        public RotateFilter(float angleInDegrees)
        {
            this.x0 = 50;
            this.y0 = 50;
            this.angle = angleInDegrees * (float)Math.PI / 180; // Преобразуем угол из градусов в радианы
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int k, int l)
        {
            // Применяем формулы поворота
            int x = (int)((k - x0) * Math.Cos(angle) - (l - y0) * Math.Sin(angle) + x0);
            int y = (int)((k - x0) * Math.Sin(angle) + (l - y0) * Math.Cos(angle) + y0);

            if (x >= sourceImage.Width || x < 0 || y >= sourceImage.Height || y < 0)
            {
                return Color.Black;
            }

            return sourceImage.GetPixel(x, y);
        }
    }
}
