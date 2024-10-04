using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace graphics
{
    public partial class mainForm : Form
    {
        private string RemoveDecimalEnding(string input)
        {
            if (input.EndsWith(".00"))
            {
                return input.Substring(0, input.Length - 3);
            }
            return input;
        }

        private string ReplaceDotsWithCommas(string input)
        {
            return input.Replace('.', ',');
        }
        public mainForm()
        {
            InitializeComponent();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string[] sizes = File.ReadAllLines(Path.Combine(basePath, "../../../src/times/i.txt"));
            string[] times0 = File.ReadAllLines(Path.Combine(basePath, "../../../src/times/times_0.txt"));
            string[] times1 = File.ReadAllLines(Path.Combine(basePath, "../../../src/times/times_1.txt"));
            string[] times2 = File.ReadAllLines(Path.Combine(basePath, "../../../src/times/times_2.txt"));

            sizes = sizes.Select(RemoveDecimalEnding).ToArray();
            times0 = times0.Select(ReplaceDotsWithCommas).ToArray();
            times1 = times1.Select(ReplaceDotsWithCommas).ToArray();
            times2 = times2.Select(ReplaceDotsWithCommas).ToArray();

            int[] matrixSizes = sizes.Select(int.Parse).ToArray();
            double[] methodTimes0 = times0.Select(double.Parse).ToArray();
            double[] methodTimes1 = times1.Select(double.Parse).ToArray();
            double[] methodTimes2 = times2.Select(double.Parse).ToArray();

            Series series_0 = chart1.Series["Классический"];
            Series series_1 = chart1.Series["Виноград"];
            Series series_2 = chart1.Series["Оптимизированный Виноград"];

            series_0.Points.DataBindXY(matrixSizes, methodTimes0);
            series_1.Points.DataBindXY(matrixSizes, methodTimes1);
            series_2.Points.DataBindXY(matrixSizes, methodTimes2);
        }
    }
}
