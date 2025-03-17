using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class TopHatFilter : MorphologicalFilter
    {
        private int scaleFactor = 3;

        public TopHatFilter(int size) : base(size) { }

        private Bitmap PerformOpening(Bitmap sourceImage, BackgroundWorker worker)
        {
            ErosionFilter erosion = new ErosionFilter(kernelSize);
            Bitmap erodedImage = erosion.processImage(sourceImage, worker);

            DilationFilter dilation = new DilationFilter(kernelSize);
            Bitmap openedImage = dilation.processImage(erodedImage, worker);

            return openedImage;
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap openedImage = PerformOpening(sourceImage, worker);

            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Color originalColor = sourceImage.GetPixel(x, y);
                    Color openedColor = openedImage.GetPixel(x, y);

                    int r = Clamp((originalColor.R - openedColor.R) * scaleFactor, 0, 255);
                    int g = Clamp((originalColor.G - openedColor.G) * scaleFactor, 0, 255);
                    int b = Clamp((originalColor.B - openedColor.B) * scaleFactor, 0, 255);

                    resultImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return resultImage;
        }
    }
}
