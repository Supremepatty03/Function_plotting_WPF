using Lab3_WPF.File_manager;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab3_WPF
{
    public partial class Page2 : Page
    {
        private double lastA, lastC, lastStep, lastStart, lastEnd;
        private bool lastFlag;
        private Dictionary<double, List<double>> lastValues = new();
        public Page2()
        {
            InitializeComponent();
            Loaded += (_, __) => DrawGraph(); // Автоматически при загрузке
            this.DataContext = Properties.Settings.Default;
        }
        private void Page2_Unloaded(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        private void WelcomeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ShowWelcomeScreen = true;
            Properties.Settings.Default.Save();
        }

        private void WelcomeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ShowWelcomeScreen = false;
            Properties.Settings.Default.Save();
        }
        private void PlotButton_Click(object sender, RoutedEventArgs e)
        {
            DrawGraph();

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!lastFlag)
            {
                MessageBox.Show("Нет данных для сохранения. Сначала постройте график.");
                return;
            }

            FileSaver.SaveToFile(lastA, lastC, lastStep, lastStart, lastEnd, lastValues, lastFlag);
        }
        private void SaveGraph_Click(object sender, RoutedEventArgs e)
        {
            if (!lastFlag)
            {
                MessageBox.Show("Нет данных для сохранения. Сначала постройте график.");
                return;
            }

            GraphExporter.SaveCanvasAsImage(Graph, lastFlag);
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileLoader.LoadFromFile(out double a, out double c, out double step, out double start, out double end))
            {
                InputA.Text = a.ToString();
                InputC.Text = c.ToString();
                InputStep.Text = step.ToString();
                InputXStart.Text = start.ToString();
                InputXEnd.Text = end.ToString();

                MessageBox.Show("Данные успешно загружены!", "Загрузка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void DrawGraph()
        {
            double start = default, a = default, c = default, step = default, end = default;

            if (!(Addons4Graph.InputCheck(InputA.Text, InputC.Text, InputStep.Text, InputXStart.Text, InputXEnd.Text,
                out a, out c, out step, out start, out end))) { lastFlag = false; return; }

            double width = Graph.ActualWidth;
            double height = Graph.ActualHeight;

            if (width == 0 || height == 0)
            {
                width = 800;
                height = 400;
            }
            Graph.Children.Clear();

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            for (double x = start; x<=end; x += step)
            {
                double y_upper = Cassini.CassiniUpper(x, a, c);
                if (y_upper > maxY) maxY = y_upper;
                double y_lower = Cassini.CassiniLower(x, a, c);
                if (y_lower < minY) minY = y_lower;
            }
            double y_range = maxY - minY;
            if (y_range == 0) y_range = 1;

            double scaleX = width / (end - start);
            double scaleY = height / y_range;

            Polyline upperGraph = new Polyline { Stroke = Brushes.Blue, StrokeThickness = 2 };
            Polyline lowerGraph = new Polyline { Stroke = Brushes.Blue, StrokeThickness = 2 };

            Dictionary<double, List<Double>> values = new Dictionary<double, List<Double>>();
            for (double x = start; x <= end; x += step)
            {
                values[x] = new List<Double>();
                double y_upper = Cassini.CassiniUpper(x, a, c);
                if (!double.IsNaN(y_upper))
                {
                    double screenX = (x - start) * scaleX;
                    double screenYUpper = height - (y_upper - minY) * scaleY;
                    upperGraph.Points.Add(new Point(screenX, screenYUpper));
                    values[x].Add(y_upper);
                }

                double y_lower = Cassini.CassiniLower(x, a, c);
                if (!double.IsNaN(y_lower))
                {
                    double screenX = (x - start) * scaleX;
                    double screenYLower = height - (y_lower - minY) * scaleY;
                    lowerGraph.Points.Add(new Point(screenX, screenYLower));
                    values[x].Add(y_lower);
                }
            }

            if (upperGraph.Points.Count == 0)
            {
                MessageBox.Show("Точки не попали в область интервала или отсутствуют");
                lastFlag = false;
                return;
            }

            Graph.Children.Add(upperGraph);
            Graph.Children.Add(lowerGraph);

            // Добавим оси
            DrawAxes(start, end, minY, maxY, scaleX, scaleY, height);

            OutputPanel.Children.Clear();

            Addons4Graph.DrawTable(values, OutputPanel);

            lastA = a;
            lastC = c;
            lastStep = step;
            lastStart = start;
            lastEnd = end;
            lastValues = values;
            lastFlag = true;
        }
        private void DrawAxes(double xStart, double xEnd, double yMin, double yMax, double scaleX, double scaleY, double height)
        {
            // Ось X (y = 0)
            if (yMin <= 0 && yMax >= 0)
            {
                double y0 = height - (0 - yMin) * scaleY;
                Line xAxis = new Line
                {
                    X1 = 0,
                    Y1 = y0,
                    X2 = Graph.ActualWidth,
                    Y2 = y0,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                Graph.Children.Add(xAxis);
            }

            // Ось Y (x = 0)
            if (xStart <= 0 && xEnd >= 0)
            {
                double x0 = (0 - xStart) * scaleX;
                Line yAxis = new Line
                {
                    X1 = x0,
                    Y1 = 0,
                    X2 = x0,
                    Y2 = height,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                Graph.Children.Add(yAxis);
            }
        }
    }
}
