using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace src
{
    public partial class Form1 : Form
    {
        private int x = 8116;
        private int N;
        private int[] A;
        private delegate int SearchDelegate(int[] array, int target, out int comparisons);
        private void InitN()
        {
            N = x / 8 + ((x >> 2) % 10 == 0 ? x % 1000 : ((x >> 2) % 10 * (x % 10) + (x >> 1) % 10));
        }
        private void InitA()
        {
            A = new int[N];
        }
        public Form1()
        {
            InitN();
            InitA();
            InitializeComponent();
        }

        private void btn_forward_nums_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < N; i++)
            {
                A[i] = i + 1;
            }
            lbl_n.Text = $"Кол-во элементов: {N}";
            lbl_content.Text = $"Заполнен: подряд идущими";
        }

        private void btn_random_nums_Click_1(object sender, EventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < N; i++)
            {
                A[i] = random.Next(1, 10000);
            }
            Array.Sort(A);
            lbl_n.Text = $"Кол-во элементов: {N}";
            lbl_content.Text = $"Заполнен: случайными";
        }

        private int FullSearch(int[] array, int target, out int comparisons)
        {
            comparisons = 0;
            for (int i = 0; i < N; i++)
            {
                comparisons++;
                if (array[i] == target)
                    return i;
            }
            return -1;
        }

        private int BinarySearch(int[] array, int target, out int comparisons)
        {
            comparisons = 0;
            int left = 0, right = N - 1;

            while (left <= right)
            {
                comparisons++;
                int mid = (left + right) / 2;
                if (array[mid] == target)
                    return mid;
                if (array[mid] < target)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            return -1;
        }

        private void SearchElement(SearchDelegate searchMethod)
        {
            int target;
            if (int.TryParse(textBox1.Text, out target))
            {
                int comparisons;
                int result = searchMethod(A, target, out comparisons);

                if (result != -1)
                {
                    lbl_state.Text = $"Элемент: найден на позиции {result}";
                    lbl_comp.Text = $"Кол-во сравнений: {comparisons}";
                }
                else
                {
                    lbl_state.Text = $"Элемент: не найден";
                    lbl_comp.Text = $"Кол-во сравнений: {comparisons}";
                }
            }
            else
                MessageBox.Show("Некорректный ввод числа.");
        }

        private void btn_full_search_Click_1(object sender, EventArgs e)
        {
            SearchElement(FullSearch);
        }
        private void btn_bin_search_Click_1(object sender, EventArgs e)
        {
            SearchElement(BinarySearch);
        }

        private void btn_gisto_Click_1(object sender, EventArgs e)
        {
            BuildHistogram(FullSearch, chart1, "Полный перебор");
            BuildHistogram(BinarySearch, chart2, "Бинарный поиск");
            BuildSortedHistogram(chart3, "Бинарный поиск");
        }

        private void BuildHistogram(SearchDelegate searchMethod, Chart chart, string title)
        {
            chart.Series.Clear();
            Series series = new Series(title);
            series.ChartType = SeriesChartType.Column;
            chart.Series.Add(series);

            for (int i = 0; i < N; i++)
            {
                int comparisons;
                searchMethod(A, A[i], out comparisons);
                series.Points.AddXY(i, comparisons);
            }
        }

        private void BuildSortedHistogram(Chart chart, string title)
        {
            chart.Series.Clear();
            Series series = new Series(title);
            series.ChartType = SeriesChartType.Column;
            chart.Series.Add(series);
            int[] sortedIndices = GetSortedIndicesByMiddle(N);
            List<(int index, int comparisons)> comparisonsList = new List<(int, int)>();

            for (int i = 0; i < N; i++)
            {
                int comparisons;
                BinarySearch(A, A[sortedIndices[i]], out comparisons);
                comparisonsList.Add((sortedIndices[i], comparisons));
            }

            comparisonsList.Sort((a, b) => a.comparisons.CompareTo(b.comparisons));

            for (int i = 0; i < comparisonsList.Count; i++)
            {
                DataPoint point = new DataPoint(i, comparisonsList[i].comparisons);
                point.AxisLabel = comparisonsList[i].index.ToString();
                series.Points.Add(point);
            }
        }
        private int[] GetSortedIndicesByMiddle(int n)
        {
            int[] sortedIndices = new int[n];
            int middle = n / 2;
            int left = middle - 1;
            int right = middle + 1;

            sortedIndices[0] = middle;
            int index = 1;

            while (left >= 0 || right < n)
            {
                if (left >= 0)
                {
                    sortedIndices[index++] = left--;
                }
                if (right < n)
                {
                    sortedIndices[index++] = right++;
                }
            }

            return sortedIndices;
        }

        private void btnSaveAsVector_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "EMF Files (*.emf)|*.emf";
                saveFileDialog.DefaultExt = "emf";
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Graphics g = CreateGraphics())
                    {
                        // Создаем объект для хранения векторного изображения (EMF)
                        using (Metafile mf = new Metafile(saveFileDialog.FileName, g.GetHdc()))
                        {
                            using (Graphics mg = Graphics.FromImage(mf))
                            {
                                // Копируем график Chart в векторный формат
                                Rectangle chartRect = new Rectangle(0, 0, chart1.Width, chart1.Height);
                                chart1.Printing.PrintPaint(mg, chartRect);
                            }
                        }
                        g.ReleaseHdc();
                    }
                    MessageBox.Show("Гистограмма сохранена как EMF-файл.");
                }
            }
        }

        private void btn_save_gisto_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "EMF Files (*.emf)|*.emf";
                saveFileDialog.DefaultExt = "emf";
                saveFileDialog.AddExtension = true;

                saveFileDialog.FileName = "chart1.emf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    SaveChartAsEmf(chart1, saveFileDialog.FileName);

                saveFileDialog.FileName = "chart2.emf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    SaveChartAsEmf(chart2, saveFileDialog.FileName);

                saveFileDialog.FileName = "chart3.emf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    SaveChartAsEmf(chart3, saveFileDialog.FileName);

                MessageBox.Show("Все гистограммы сохранены!");
            }
        }
        private void SaveChartAsEmf(Chart chart, string filePath)
        {
            using (Graphics g = CreateGraphics())
            {
                using (Metafile mf = new Metafile(filePath, g.GetHdc()))
                {
                    using (Graphics mg = Graphics.FromImage(mf))
                    {
                        Rectangle chartRect = new Rectangle(0, 0, chart.Width, chart.Height);
                        chart.Printing.PrintPaint(mg, chartRect);
                    }
                }
                g.ReleaseHdc();
            }
        }
    }
}

