using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class OpeningFilter : Filters
    {
        private int kernelSize;

        public OpeningFilter(int size)
        {
            kernelSize = size;
        }

        public Bitmap ProcessOpening(Bitmap sourceImage, BackgroundWorker worker)
        {
            // Шаг 1: Эрозия
            ErosionFilter erosion = new ErosionFilter(kernelSize);
            Bitmap erodedImage = erosion.processImage(sourceImage, worker);

            // Шаг 2: Дилатация
            DilationFilter dilation = new DilationFilter(kernelSize);
            Bitmap openedImage = dilation.processImage(erodedImage, worker);

            return openedImage;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            return ProcessOpening(sourceImage, worker);
        }
    }
}
