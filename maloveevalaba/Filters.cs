using maloveevalaba;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    abstract class Filters
    {
        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y);
        public virtual Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i<sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i/resultImage.Width*100));
                if (worker.CancellationPending) return null;
                for (int j = 0; j<sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;

        }
        public int Clamp(int value, int min, int max)
        {
            if (value<min) return min;
            if (value>max) return max;
            return value;
        }
    }
    class InvertFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255-sourceColor.R, 255-sourceColor.G, 255-sourceColor.B);
            return resultColor;
        }
    }
    class MatrixFilter : Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0)/2;
            int radiusY = kernel.GetLength(1)/2;
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            for (int l = -radiusY; l<=radiusY; l++)
            {
                for (int k = -radiusX; k<=radiusX; k++)
                {
                    int idX = Clamp(x+k, 0, sourceImage.Width-1);
                    int idY = Clamp(y+l, 0, sourceImage.Height-1);
                    Color neighborColor=sourceImage.GetPixel(idX, idY);
                    resultR+=neighborColor.R* kernel[k+radiusX, l+radiusY];
                    resultG+=neighborColor.G* kernel[k+radiusX, l+radiusY];
                    resultB+=neighborColor.B* kernel[k+radiusX, l+radiusY];
                }
            }
            return Color.FromArgb(Clamp((int)resultR, 0, 255), Clamp((int)resultG, 0, 255), Clamp((int)resultB, 0, 255));
        }
    }
}
//класс для морфологии
class MorphologicalFilter : Filters
{
    protected int[,] kernel;
    protected int kernelSize;

    public MorphologicalFilter(int size)
    {
        kernelSize = size;
        kernel = new int[size, size];
        // Заполняем ядро (например, единичное ядро для операцій морфологии)
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                kernel[i, j] = 1;
            }
        }
    }

    // Базовая функция для выполнения операций морфологии
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int halfKernelSize = kernelSize / 2;
        int minValue = 255;
        int maxValue = 0;

        for (int i = -halfKernelSize; i <= halfKernelSize; i++)
        {
            for (int j = -halfKernelSize; j <= halfKernelSize; j++)
            {
                int idX = Clamp(x + i, 0, sourceImage.Width - 1);
                int idY = Clamp(y + j, 0, sourceImage.Height - 1);
                Color neighborColor = sourceImage.GetPixel(idX, idY);
                int gray = (int)(0.299 * neighborColor.R + 0.587 * neighborColor.G + 0.114 * neighborColor.B);

                if (kernel[i + halfKernelSize, j + halfKernelSize] == 1)
                {
                    minValue = Math.Min(minValue, gray);
                    maxValue = Math.Max(maxValue, gray);
                }
            }
        }

        // Для dilation возвращаем максимальное значение
        return Color.FromArgb(maxValue, maxValue, maxValue);
    }
}

class ReferenceColorCorrectionFilter : Filters
{
    private Color referenceColor;

    public ReferenceColorCorrectionFilter(Color referenceColor)
    {
        this.referenceColor = referenceColor;
    }

    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        Color sourceColor = sourceImage.GetPixel(x, y);

        // Вычисление корректировки для каждого канала (R, G, B)
        int newR = Clamp((int)(sourceColor.R * referenceColor.R /15), 0, 255);
        int newG = Clamp((int)(sourceColor.G * referenceColor.G / 15), 0, 255);
        int newB = Clamp((int)(sourceColor.B * referenceColor.B / 15), 0, 255);

        return Color.FromArgb(newR, newG, newB);
    }
}
class MaximumFilter : Filters
{
    private int radius;

    public MaximumFilter(int radius = 1)
    {
        this.radius = radius;
    }

    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int maxR = 0, maxG = 0, maxB = 0;

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                int newX = Clamp(x + i, 0, sourceImage.Width - 1);
                int newY = Clamp(y + j, 0, sourceImage.Height - 1);
                Color color = sourceImage.GetPixel(newX, newY);

                maxR = Math.Max(maxR, color.R);
                maxG = Math.Max(maxG, color.G);
                maxB = Math.Max(maxB, color.B);
            }
        }

        return Color.FromArgb(maxR, maxG, maxB);
    }
}
class TransferFilter : MatrixFilter
{
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int k, int l)
    {
        int x = k + 50;
        int y = l;

        if (x >= sourceImage.Width || x < 0 || y >= sourceImage.Height || y < 0)
        {
            return Color.Black;
        }

        return sourceImage.GetPixel(x, y);
    }
}


