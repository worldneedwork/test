using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        private List<double> voltages = new List<double>();
        public Form1()
        {
            InitializeComponent();
      
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = @"test.txt";

            // 读取文件中包含 "CellVs:" 的行，并提取数据
            List<double> dataPoints = new List<double>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    int index = line.IndexOf("CellVs:");
                    if (index >= 0)
                    {
                        string dataString = line.Substring(index + 7).Trim();
                        double data = double.Parse(dataString);
                        dataPoints.Add(data);
                    }
                }
            }
            dataPoints = dataPoints.Select(d => Math.Round(d, 3)).ToList();

            // 统计数据点的频率
            Dictionary<double, int> frequency = new Dictionary<double, int>();
            foreach (double dataPoint in dataPoints)
            {
                if (frequency.ContainsKey(dataPoint))
                {
                    frequency[dataPoint]++;
                }
                else
                {
                    frequency[dataPoint] = 1;
                }
            }

            // 清空绘图区域
            chart1.Series.Clear();

            // 添加柱状图系列
            Series series = new Series("Frequency", 0);
            series.ChartType = SeriesChartType.Column;
            foreach (var kvp in frequency.OrderBy(k => k.Key))
            {
                series.Points.AddXY(kvp.Key, kvp.Value);
            }
            chart1.Series.Add(series);

            // 设置 X 轴和 Y 轴的标签
            chart1.ChartAreas[0].AxisX.Title = "Data Points";
            chart1.ChartAreas[0].AxisY.Title = "Frequency";

            // 自动调整 Y 轴的最大值和最小值
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
            chart1.ChartAreas[0].AxisX.Interval = 0.01; // 设置刻度间隔为1
            chart1.ChartAreas[0].AxisX.Minimum = 3.40; // 设置最小值
            chart1.ChartAreas[0].AxisX.Maximum = 3.62; // 设置最大值

            // 显示图形
            chart1.Visible = true;
        }

    }
}
