using Microsoft.Win32;
using Modul_13.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Modul_13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }

        public  ICollectionView CollectionView { get; private set; }

        public MainWindow()
        {
            ViewModel = ViewModel ?? new MainWindowViewModel(@"C:\Users\nikonovia\OneDrive\Будущее\Skillbox\Програмирование\ДЗ 12.2\Modul_12.2\Data\ClientsBank.json");

            this.DataContext = ViewModel;

            CollectionView = CollectionViewSource.GetDefaultView(ViewModel.BankRepository);
            
            InitializeComponent(); 
        }

        private void CloseWindows(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Выбор функицонала консультант / менаджер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccessLevel_ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (AccessLevel_ComboBox.SelectedIndex)
            {
                case 0: //консультант

                    DataClients.ItemsSource = CollectionViewSource.GetDefaultView(ViewModel.Consultant.ViewClientsData(ViewModel.BankRepository));

                    break;

                case 1: //менждер

                    DataClients.ItemsSource = CollectionView;

                    break;

                default:
                    break;

            }
        }

        /// <summary>
        /// Для анимации закрытия списка изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            СhangesClient.Visibility = Visibility.Collapsed;
            ListChanges_Label.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Для анимации открытия списка изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
            СhangesClient.Visibility = Visibility.Visible;
            ListChanges_Label.Visibility = Visibility.Collapsed;
        }

        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            foreach (var client in ViewModel.BankRepository)
            {
                if (client.Owner.IsChanged == true) { e.CanExecute = true; break; }

                else e.CanExecute = false;
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var saveDlg = new SaveFileDialog { Filter = "Text files|*.json" };

            if (true == saveDlg.ShowDialog())
            {
                string fileName = saveDlg.FileName;

                string json = JsonConvert.SerializeObject(DataClients.ItemsSource, Formatting.Indented);

                File.WriteAllText(fileName, json);

                foreach (var client in ViewModel.BankRepository)
                {
                    client.Owner.IsChanged = false;
                }
            }

        }

        /// <summary>
        /// Перемещение окна в пространестве рабочего стола
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { this.DragMove(); }
        }

        private bool IsMaximized = false;
        /// <summary>
        /// Сворачивание окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gride_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) 
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Height = 800;
                    this.Width = 1250;
                    IsMaximized= false;
                }
                else 
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized= true;
                }
            }
        }
    }
}
