using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maloveevalaba
{
    class SvetKraya : MatrixFilter
    {
        private MedianFilter medianFilter;
        private SobelFilter sobelFilter;
        private MaximumFilter maximumFilter;

        public SvetKraya(int medianRadius = 1)
        {
            medianFilter = new MedianFilter(medianRadius);
            sobelFilter = new SobelFilter();  // фильтр Собеля по оси X
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
}
