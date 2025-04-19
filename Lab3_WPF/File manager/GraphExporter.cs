using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_WPF.File_manager
{
    using Microsoft.Win32;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class GraphExporter
    {
        public static void SaveCanvasAsImage(Canvas canvas, bool flag)
        {
            // Открываем диалог сохранения
            SaveFileDialog dlg = new SaveFileDialog
            {
                Title = "Сохранить график как изображение",
                Filter = "PNG Image|*.png",
                FileName = "График.png"
            };

            if (dlg.ShowDialog() == true)
            {
                // Создаем рендер изображения
                Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
                double dpi = 96d;

                RenderTargetBitmap rtb = new RenderTargetBitmap(
                    (int)bounds.Width, (int)bounds.Height,
                    dpi, dpi, PixelFormats.Pbgra32);

                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext ctx = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(canvas);
                    ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                }
                rtb.Render(dv);

                // Сохраняем в файл
                using (FileStream fs = new FileStream(dlg.FileName, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(fs);
                }

                MessageBox.Show("График успешно сохранён!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
