﻿using Modul_13.Cmds;
using Modul_13.Models;
using Modul_13.View;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.ViewModels
{
    public class MainWindowViewModel:ViewModel
    {
        #region Свойства
        private BankRepository bankRepository;
        public BankRepository BankRepository 
        {
            get=> bankRepository;
            private set 
            {
                Set(ref bankRepository, value, "BankRepository");
            }
        }

        /// <summary>
        /// Выбранный из базы ClientsRepository клиент
        /// определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public MainWindow MWindow { get; }

        private BankClient<Account> currentClient = null;
        public BankClient<Account> CurrentClient { get => currentClient = this.MWindow.DataClients.SelectedItem as BankClient<Account>; }

        /// <summary>
        /// Уровень доступа к базе данных для консультанта и менаджера, 
        /// определяется на основании выбраного параметра в элементе ComboBox "AccessLevel_ComboBox"
        /// принадлежащего MainWindow
        /// </summary>
        public int AccessLevel
        {
            get
            {
                int? index = this.MWindow.AccessLevel_ComboBox.SelectedIndex;

                int s = index ?? 0;

                return s;
            }
        }

        public Consultant Consultant { get; } 

        public Meneger Meneger { get; }
        #endregion

        //конструктор
        public MainWindowViewModel(string path, MainWindow mWindow) 
        {
            this.BankRepository = new BankRepository(path);

            this.MWindow = mWindow;

            this.Consultant = new Consultant();

            this.Meneger = new Meneger();

            //this.BankRepository.CollectionChanged += BankRepository_CollectionChanged;
        }

       //private void BankRepository_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
       // {
       //     // They removed something. 
       //     if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
       //     {
                
       //         foreach (BankClient<Account> p in e.OldItems)
       //         {
       //             Console.WriteLine(p.ToString());
       //         }
                
       //     }

            
       //     if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
       //     {
       //         foreach (BankClient<Account> p in e.NewItems)
       //         {
       //             logClient.Add(p.ToString());

       //         }
       //     }
       // }

        #region Команды
        private RelayCommand newClientAddCommand = null;
        public RelayCommand NewClientAddCommand => 
            newClientAddCommand ?? (newClientAddCommand = new RelayCommand(AddNewClient, CanAddClient));

        private RelayCommand deleteClientCommand = null;
        public RelayCommand DeleteClientCommand => 
            deleteClientCommand ?? (deleteClientCommand = new RelayCommand(DeleteClient, CanDeleteClient));

        #endregion

        private bool CanDeleteClient()
        {
            if (AccessLevel == 1 && CurrentClient != null) { return true; }
            return false;
        }

        private bool CanAddClient()
        {
            if (AccessLevel == 1) { return true; }

            return false;
        }

        /// <summary>
        /// Метод удаления нового клиента
        /// </summary>
        private void DeleteClient()
        {
            if (CurrentClient != null) { BankRepository.Remove(CurrentClient); }
        }

        /// <summary>
        /// Метод добавления нового клиенита
        /// </summary>
        private void AddNewClient()
        {
            NewClientWindow _windowNewClient = new NewClientWindow();

            _windowNewClient.Owner = this.MWindow;

            _windowNewClient.ShowDialog();

            if (_windowNewClient.DialogResult == true)
            {
                //if (!BankRepository.Contains(_windowNewClient.NewClient))
                //{

                    BankClient<Account> newAccount = new BankClient<Account>(_windowNewClient.NewClient);

                    BankRepository.Add(newAccount);

                //}

                //else ShowStatusBarText("Клиент с такими данными уже существует");
            }
        }

        public void ShowStatusBarText(string message)
        {
            TextBlock statusBar = Application.Current.MainWindow.FindName("StatusBarText") as TextBlock;
            
            statusBar.Text = message;

            var timer = new System.Timers.Timer();

            timer.Interval = 3000;

            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {
                timer.Stop();
                //удалите текст сообщения о состоянии с помощью диспетчера, поскольку таймер работает в другом потоке
                MWindow.Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusBar.Text = "";
                }));

            };
            timer.Start();
        }

    }
}
