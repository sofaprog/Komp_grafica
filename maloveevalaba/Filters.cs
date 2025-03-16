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

class brightness : Filters
{
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        Color sourceColor = sourceImage.GetPixel(x, y);
        int k = 50;
        int r = sourceColor.R+k;
        int g =sourceColor.G+k;
        int b = sourceColor.B+k;
        r = Clamp(r, 0, 255);
        g = Clamp(g, 0, 255);
        b = Clamp(b, 0, 255);
        return Color.FromArgb(r, g, b);
    }
}
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

class sharpnessFilter : MatrixFilter
{
    private float[,] kernel = new float[,] {
         { 0, -1,  0 },
            {-1,  5, -1 },
            { 0, -1,  0 }
    };
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int radiusX = kernel.GetLength(0) / 2; 
        int radiusY = kernel.GetLength(1) / 2; 

        float resultR = 0;
        float resultG = 0;
        float resultB = 0;

        for (int l = -radiusY; l <= radiusY; l++)
        {
            for (int k = -radiusX; k <= radiusX; k++)
            {
                int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                Color neighborColor = sourceImage.GetPixel(idX, idY);
                resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
            }
        }

        resultR = Clamp((int)resultR, 0, 255);
        resultG = Clamp((int)resultG, 0, 255);
        resultB = Clamp((int)resultB, 0, 255);

        return Color.FromArgb((int)resultR, (int)resultG, (int)resultB);
    }

}
class embossingFilter : MatrixFilter
{
    private float[,] kernel = new float[,] {
         { 0, 1,  0 },
          {1,  0, -1 },
          { 0, -1,  0 }
    };
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int radiusX = kernel.GetLength(0) / 2;
        int radiusY = kernel.GetLength(1) / 2;

        float resultR = 0;
        float resultG = 0;
        float resultB = 0;

        for (int l = -radiusY; l <= radiusY; l++)
        {
            for (int k = -radiusX; k <= radiusX; k++)
            {
                int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                Color neighborColor = sourceImage.GetPixel(idX, idY);
                resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
            }
        }

        resultR = Clamp((int)resultR, 0, 255);
        resultG = Clamp((int)resultG, 0, 255);
        resultB = Clamp((int)resultB, 0, 255);

        return Color.FromArgb((int)resultR, (int)resultG, (int)resultB);
    }

}
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
class operatorPruita : MatrixFilter
{
    private float[,] kernelY = new float[,] {
        {-1, -1, -1},
        {0, 0, 0},
        {1, 1, 1}
    };

    private float[,] kernelX = new float[,] {
        {-1, 0, -1},
        { -1,  0,  1},
        { -1,  0,  1}
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
class GlassEffectFilter : Filters
{
    private Random random = new Random();

    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int offset = 2; 

        int randomX = Clamp(x + random.Next(-offset, offset + 1), 0, sourceImage.Width - 1);
        int randomY = Clamp(y + random.Next(-offset, offset + 1), 0, sourceImage.Height - 1);

        Color neighborColor = sourceImage.GetPixel(randomX, randomY);

        return neighborColor;
    }
}
class transferFilter : Filters
{
    private Random random = new Random();

    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int offset = 2;

        int randomX = Clamp(x+50, 0, sourceImage.Width - 1);
        int randomY = Clamp(y+10, 0, sourceImage.Height - 1);

        Color neighborColor = sourceImage.GetPixel(randomX, randomY);

        return neighborColor;
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

class ErosionFilter : MorphologicalFilter
{
    public ErosionFilter(int size) : base(size) { }

    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int halfKernelSize = kernelSize / 2;
        int minValue = 255;

        for (int i = -halfKernelSize; i <= halfKernelSize; i++)
        {
            for (int j = -halfKernelSize; j <= halfKernelSize; j++)
            {
                int idX = Clamp(x + i, 0, sourceImage.Width - 1);
                int idY = Clamp(y + j, 0, sourceImage.Height - 1);
                Color neighborColor = sourceImage.GetPixel(idX, idY);
                int gray = (int)(0.299 * neighborColor.R + 0.587 * neighborColor.G + 0.114 * neighborColor.B);
                minValue = Math.Min(minValue, gray);
            }
        }
        return Color.FromArgb(minValue, minValue, minValue);
    }
}
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
class MedianFilter : Filters
{
    private int radius;

    //размеры окна
    public MedianFilter(int radius = 1)
    {
        this.radius = radius;
    }

    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        List<int> rValues = new List<int>();
        List<int> gValues = new List<int>();
        List<int> bValues = new List<int>();

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                int newX = Clamp(x + i, 0, sourceImage.Width - 1);
                int newY = Clamp(y + j, 0, sourceImage.Height - 1);
                Color color = sourceImage.GetPixel(newX, newY);

                rValues.Add(color.R);
                gValues.Add(color.G);
                bValues.Add(color.B);
            }
        }

        rValues.Sort();
        gValues.Sort();
        bValues.Sort();

        int medianIndex = rValues.Count / 2;

        return Color.FromArgb(rValues[medianIndex], gValues[medianIndex], bValues[medianIndex]);
    }
}
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
class GrayWorldFilter : Filters
{
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        Color sourceColor = sourceImage.GetPixel(x, y);

        // Вычисление среднего значения каналов R, G, B
        int average = (sourceColor.R + sourceColor.G + sourceColor.B) / 3;

        return Color.FromArgb(average, average, average);
    }
}
class PerfectReflectorFilter : Filters
{
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        Color sourceColor = sourceImage.GetPixel(x, y);

        // Отражение цвета: инвертируем каждый канал
        return Color.FromArgb(255 - sourceColor.R, 255 - sourceColor.G, 255 - sourceColor.B);
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

//светящиеся края
class SvetKraya : MatrixFilter
{
    private MedianFilter medianFilter;
    private SobelFilter sobelFilter;
    private MaximumFilter maximumFilter;

    public SvetKraya(int medianRadius = 1)
    {
        medianFilter = new MedianFilter(medianRadius);
        sobelFilter = new  SobelFilter();  // фильтр Собеля по оси X
        maximumFilter = new MaximumFilter();
    }

    public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
    {

        Bitmap medianImage = medianFilter.processImage(sourceImage, worker);
        Bitmap edgesImage = sobelFilter.processImage(medianImage, worker);
        Bitmap glowingEdgesImage = maximumFilter.processImage(edgesImage, worker);

        return glowingEdgesImage;
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
//волны
class WavesFilter1 : Filters
{
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int newX = x;
        int newY = Clamp(y + (int)(50 * Math.Sin(Math.PI * x / 20)), 0, sourceImage.Height - 1);

        return sourceImage.GetPixel(newX, newY);
    }
}
class WavesFilter2 : Filters
{
    protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        int newX = Clamp(x + (int)(20 * Math.Sin(2 * Math.PI * y / 60)), 0, sourceImage.Width - 1);
        int newY = y;
        return sourceImage.GetPixel(newX, newY);
    }
}
internal class BlurFilter2 : MatrixFilter
{
    public BlurFilter2()
    {
        int sizeX = 3;
        int sizeY = 3;
        kernel = new float[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                kernel[i, j] = 1.0f / (float)(sizeX * sizeY);
            }
        }
    }
}

