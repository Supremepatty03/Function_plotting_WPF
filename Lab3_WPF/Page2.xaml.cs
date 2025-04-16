using System;
using System.Collections.Generic;
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
        public Page2()
        {
            InitializeComponent();
            Loaded += (_, __) => DrawGraph(); // Автоматически при загрузке
        }
        private void PlotButton_Click(object sender, RoutedEventArgs e)
        {
            DrawGraph();

        }
        private void DrawGraph()
        {
            double start = default, a = default, c = default, step = default, end = default;
            if (!(InputHandler.TryParse(InputA.Text, out a) &&
                InputHandler.TryParse(InputC.Text, out c) &&
                InputHandler.TryParse(InputStep.Text, out step) &&
                InputHandler.TryParse(InputXStart.Text, out start) &&
                InputHandler.TryParse(InputXEnd.Text, out end)))
            { 
                MessageBox.Show("Ошибка ввода. Проверьте значения.");
                return;
            }
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

            for (double x = start; x <= end; x += step)
            {
                double y_upper = Cassini.CassiniUpper(x, a, c);
                if (!double.IsNaN(y_upper))
                {
                    double screenX = (x - start) * scaleX;
                    double screenYUpper = height - (y_upper - minY) * scaleY;
                    upperGraph.Points.Add(new Point(screenX, screenYUpper));
                }

                double y_lower = Cassini.CassiniLower(x, a, c);
                if (!double.IsNaN(y_lower))
                {
                    double screenX = (x - start) * scaleX;
                    double screenYLower = height - (y_lower - minY) * scaleY;
                    lowerGraph.Points.Add(new Point(screenX, screenYLower));
                }
            }

            Graph.Children.Add(upperGraph);
            Graph.Children.Add(lowerGraph);

            // Добавим оси
            DrawAxes(start, end, minY, maxY, scaleX, scaleY, height);
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
