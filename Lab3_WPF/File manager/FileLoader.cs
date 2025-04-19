using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace Lab3_WPF.File_manager
{
    public static class FileLoader
    {
        public static bool LoadFromFile(out double a, out double c, out double step, out double start, out double end)
        {
            a = c = step = start = end = 0;

            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = "Загрузить данные",
                Filter = "Текстовые файлы (*.txt)|*.txt"
            };

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    string[] lines = File.ReadAllLines(openDialog.FileName);

                    foreach (string line in lines)
                    {
                        if (line.StartsWith("a =")) double.TryParse(line.Substring(3).Trim(), out a);
                        if (line.StartsWith("c =")) double.TryParse(line.Substring(3).Trim(), out c);
                        if (line.StartsWith("Шаг =")) double.TryParse(line.Substring(5).Trim(), out step);
                        if (line.StartsWith("Интервал:"))
                        {
                            var parts = line.Split(new string[] { "от", "до" }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length >= 2)
                            {
                                double.TryParse(parts[1].Trim(), out start);
                                double.TryParse(parts[2].Trim(), out end);
                            }
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message);
                    return false;
                }
            }

            return false;
        }
    }
}