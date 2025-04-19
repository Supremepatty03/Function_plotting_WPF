using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

namespace Lab3_WPF.File_manager
{
    public static class FileSaver
    {
        public static void SaveToFile( double a, double c, double step, double start, double end, Dictionary<double, List<double>> values, bool flag)
        {
            if (!flag)
            {
                MessageBox.Show("Нет данных для сохранения. Сначала постройте график.");
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Title = "Сохранить данные",
                Filter = "Текстовые файлы (*.txt)|*.txt",
                FileName = "Результат_Кассини.txt"
            };
            if (saveDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    // Сохраняем начальные условия
                    writer.WriteLine("Исходные данные:");
                    writer.WriteLine($"a = {a}");
                    writer.WriteLine($"c = {c}");
                    writer.WriteLine($"Шаг = {step}");
                    writer.WriteLine($"Интервал: от {start} до {end}");
                    writer.WriteLine();

                    // Сохраняем таблицу результатов
                    writer.WriteLine("Результаты (x, ±y):");
                    foreach (var point in values)
                    {
                        double x = point.Key;
                        if (point.Value.Count > 0)
                        {
                            double y = Math.Abs(point.Value[0]);
                            writer.WriteLine($"x = {x:0.####}, y = ±{y:0.####}");
                        }
                    }
                }

                MessageBox.Show("Данные успешно сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
