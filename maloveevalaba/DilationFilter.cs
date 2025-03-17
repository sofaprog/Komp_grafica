using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class DilationFilter : MorphologicalFilter
    {
        public DilationFilter(int size) : base(size) { }

        public Bitmap ProcessDilation(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending) return null;

                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            return resultImage;
        }

        // Переопределенный метод для дилатации
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int halfKernelSize = kernelSize / 2;
            int maxValue = 0;

            for (int i = -halfKernelSize; i <= halfKernelSize; i++)
            {
                for (int j = -halfKernelSize; j <= halfKernelSize; j++)
                {
                    int idX = Clamp(x + i, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + j, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    int gray = (int)(0.299 * neighborColor.R + 0.587 * neighborColor.G + 0.114 * neighborColor.B);
                    maxValue = Math.Max(maxValue, gray);
                }
            }
            return Color.FromArgb(maxValue, maxValue, maxValue);
        }
    }
}
