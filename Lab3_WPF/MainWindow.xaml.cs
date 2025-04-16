using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Lab3_WPF
{
    public partial class MainWindow : Window
    {
        private Page2 _page2 = new Page2();  // Создаём экземпляр страницы заранее

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Скрываем начальную панель
            StartPanel.Visibility = Visibility.Collapsed;

            // Показываем Frame с небольшой задержкой
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(_page2); // Загружаем страницу

            // Анимация плавного появления (fade-in)
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500)); // 500ms для плавности
            MainFrame.BeginAnimation(OpacityProperty, fadeIn);
        }
    }
}