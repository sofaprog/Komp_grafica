using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using maloveevalaba;
namespace maloveevalaba
{
    public partial class Form1 : Form
    {
        Bitmap image;
        //структура - квадрат (для примера)


        private List<Bitmap> history = new List<Bitmap>(); //история всех действий
        private int historyIndex = 0; //индекс текущего состояния
       
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
                AddNewStateToHistory();
            }
            progressBar1.Value = 0;
        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending!=true) 
                image=newImage;
        }
        private void backgroundWorker1_ProgressChanged_1(object sender, ProgressChangedEventArgs e)
        {

            progressBar1.Value=e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image=image;
                pictureBox1.Refresh();
            }
            progressBar1.Value=0;
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void размытиеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void фильтрыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void гаусаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void чернобелыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new GrayScaleFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new SepiaFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void яркостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new brightness();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void собельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new SobelFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void резкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new sharpnessFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new embossingFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void операторЩарраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new operatorsharraFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void стеклоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new GlassEffectFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void переносToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new transferFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void расширениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new DilationFilter(10);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сужениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new ErosionFilter(10);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void tophatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new TopHatFilter(20);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void открытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new OpeningFilter(20);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void закрытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new ClosingFilter(20);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void градиентToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new GradientFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void медианныйФилToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new MedianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void линейноеРастяжениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new LinearStretchingFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new GrayWorldFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void идеальныйОтражательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new PerfectReflectorFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void коррекцияСОпорнымЦветомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new PerfectReflectorFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Нет изображения для сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Сохранить изображение как",
                Filter = "PNG файлы|*.png|JPEG файлы|*.jpg|BMP файлы|*.bmp|Все файлы|*.*"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog.FileName);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.Refresh();

                // Очистить историю перед загрузкой нового изображения
                history.Clear();
                historyIndex = 0;  // Начинаем с 0, т.к. это первое состояние изображения
                AddNewStateToHistory();  // Добавить текущее изображение в историю
            }
        }

        private void отменаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (historyIndex > 0)  // Проверяем, можем ли откатиться назад
            {
                historyIndex--;  // Понижаем индекс, чтобы перейти к предыдущему состоянию
                image = new Bitmap(history[historyIndex]);  // Восстанавливаем изображение из истории
                pictureBox1.Image = image;  // Обновляем изображение в pictureBox
            }
            else
            {
                MessageBox.Show("Нет доступных изменений для отмены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddNewStateToHistory()
        {
            if (historyIndex < history.Count - 1)
            {
                // Если текущий индекс меньше длины истории, удаляем все после текущего состояния
                history.RemoveRange(historyIndex + 1, history.Count - (historyIndex + 1));
            }

            // Добавляем копию текущего изображения в историю
            history.Add(new Bitmap(image));
            historyIndex = history.Count - 1;  // Устанавливаем текущий индекс на последнее состояние
        }

        public Form1()
        {
            InitializeComponent();
            // Подписываемся на событие изменения размера формы
            this.Resize += new EventHandler(Form1_Resize);  // Добавляем эту строку
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                // Получаем текущие размеры окна
                int newWidth = this.ClientSize.Width;
                int newHeight = this.ClientSize.Height;

                // Вычисляем новые пропорциональные размеры для изображения
                float aspectRatio = (float)image.Width / image.Height;
                int newImageWidth = newWidth;
                int newImageHeight = (int)(newWidth / aspectRatio);

                // Если изображение по высоте выходит за пределы окна, то подгоняем его по высоте
                if (newImageHeight > newHeight)
                {
                    newImageHeight = newHeight;
                    newImageWidth = (int)(newHeight * aspectRatio);
                }

                // Устанавливаем новый размер изображения
                pictureBox1.Width = newImageWidth;
                pictureBox1.Height = newImageHeight;

                // Центрируем изображение в окне
                pictureBox1.Left = (newWidth - newImageWidth) / 2;
                pictureBox1.Top = (newHeight - newImageHeight) / 2;
            }
        }

        private void светящиесяКраяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new SvetKraya();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void переносToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            AddNewStateToHistory();
            Filters filter = new TransferFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void поворотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new RotateFilter(10);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void волны1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new WavesFilter1();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void волны2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new WavesFilter2();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void блюрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStateToHistory();
            Filters filter = new BlurFilter2();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void операторПрюитаToolStripMenuItem_Click(object sender, EventArgs e)
        {
                AddNewStateToHistory();
            Filters filter = new operatorPruita();
            backgroundWorker1.RunWorkerAsync(filter);
        }
    }

}


