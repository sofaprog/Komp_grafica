using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class LinearStretchingFilter : Filters
    {
        private int minBrightness, maxBrightness;

        public override Bitmap processImage(Bitmap sourceImage, System.ComponentModel.BackgroundWorker worker)
        {

            minBrightness = 255;
            maxBrightness = 0;

            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Color color = sourceImage.GetPixel(x, y);
                    int brightness = (color.R + color.G + color.B) / 3;

                    if (brightness < minBrightness) minBrightness = brightness;
                    if (brightness > maxBrightness) maxBrightness = brightness;
                }
            }

            return base.processImage(sourceImage, worker);
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);

            int r = (color.R - minBrightness) * 255 / (maxBrightness - minBrightness);
            int g = (color.G - minBrightness) * 255 / (maxBrightness - minBrightness);
            int b = (color.B - minBrightness) * 255 / (maxBrightness - minBrightness);

            return Color.FromArgb(Clamp(r, 0, 255), Clamp(g, 0, 255), Clamp(b, 0, 255));
        }
    }
}
