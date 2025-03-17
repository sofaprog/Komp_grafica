using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class ClosingFilter : Filters
    {
        private int kernelSize;

        public ClosingFilter(int size)
        {
            kernelSize = size;
        }

        public Bitmap ProcessClosing(Bitmap sourceImage, BackgroundWorker worker)
        {
            // Шаг 1: Дилатация
            DilationFilter dilation = new DilationFilter(kernelSize);
            Bitmap dilatedImage = dilation.processImage(sourceImage, worker);

            // Шаг 2: Эрозия
            ErosionFilter erosion = new ErosionFilter(kernelSize);
            Bitmap closedImage = erosion.processImage(dilatedImage, worker);

            return closedImage;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            return ProcessClosing(sourceImage, worker);
        }
    }
}
