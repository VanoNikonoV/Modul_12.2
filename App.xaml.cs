using Modul_13.ViewModels;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Modul_13
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создайте ViewModel, к которому привязывается главное окно.

            string path = @"C:\Users\nikonovia\OneDrive\Будущее\Skillbox\Програмирование\ДЗ 12.2\Modul_12.2\Data\ClientsBank.json";

            var viewModel = new MainWindowViewModel(path);

            MainWindow window = new MainWindow(viewModel); 

            //window.ViewModel = viewModel;

            //window.DataContext = viewModel;

            window.Show();
        }

    }

    
}
