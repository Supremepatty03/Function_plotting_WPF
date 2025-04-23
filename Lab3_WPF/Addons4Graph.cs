using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lab3_WPF
{
    public static class Addons4Graph
    {
        public static bool InputCheck(System.String a1, System.String c1, System.String step1, System.String start1, System.String end1,
            out double a, out double c, out double step, out double start, out double end)
        {
            a = c = step = start = end = 0;
            if (!(InputHandler.TryParse(a1, out a) &&
                InputHandler.TryParse(c1, out c) &&
                InputHandler.TryParse(step1, out step) &&
                InputHandler.TryParse(start1, out start) &&
                InputHandler.TryParse(end1, out end)))
            {
                MessageBox.Show("Ошибка ввода. Проверьте значения.");
                return false;
            }
            if (start > end)
            {
                MessageBox.Show("Ошибка ввода. Начало интервала больше конца.");
                return false;
            }
            if (step <= 0)
            {
                MessageBox.Show("Ошибка ввода. Шаг должен быть больше 0.");
                return false;
            }
            return true;
        }
        public static void DrawTable (Dictionary <double , List <double>> table, Panel OutputPanel)
        {
            foreach (var points in table)
            {
                double x = points.Key;
                if (points.Value.Count > 0)
                {
                    double y = Math.Abs(points.Value[0]);

                    TextBlock newBlock = new TextBlock
                    {
                        Text = $"x = {x:0.####}, y = ±{y:0.####}",
                        Margin = new Thickness(5, 2, 5, 2)
                    };

                    OutputPanel.Children.Add(newBlock);
                }
            }
        }
    }
}
