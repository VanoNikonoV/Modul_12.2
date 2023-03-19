using Modul_13.Interfases;
using Modul_13.Models;
using Modul_13.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.View
{
    /// <summary>
    /// Логика взаимодействия для PaneWorkingWithDeposit.xaml
    /// </summary>
    public partial class PanelWorkingWithDeposit : Page
    {
        public PanelWorkingWithDepositViewModel PanelWorkingWithDepositViewModel { get; set; }

        public PanelWorkingWithDeposit()
        {
            InitializeComponent();

            PanelWorkingWithDepositViewModel = new PanelWorkingWithDepositViewModel();

            PanelWorkingWithDepositViewModel.SumAddDeposit_TextBox = this.SumAddDeposit_TextBox;

            PanelWorkingWithDepositViewModel.SumAddNoDeposit_TextBox = this.SumAddNoDeposit_TextBox;
        }

        /// <summary>
        /// Выбор получателя платежа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedRecipient(object sender, SelectionChangedEventArgs e)
        {
            BankClient<Account> temp = this.List_SortRepository.SelectedItem as BankClient<Account>;

            if (temp != null)
            {
                if (ConditionsForListAccounts.SelectedIndex == 0)
                {
                    PanelWorkingWithDepositViewModel.Recipient = temp.Deposit;
                }
                if (ConditionsForListAccounts.SelectedIndex == 1)
                {
                    PanelWorkingWithDepositViewModel.Recipient = temp.NoDeposit;
                }
            }
        }

        /// <summary>
        /// Обновляет список клиентов с по выбранному критерию (см. Combobox ConditionsForListAccounts)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangingSelectionListAccounts(object sender, SelectionChangedEventArgs e)
        {
            if (PanelWorkingWithDepositViewModel != null)
            {
                switch (ConditionsForListAccounts.SelectedIndex)
                {
                    case 0: //Депозитные счета

                        IEnumerable<BankClient<Account>> onliDeposit = from client in PanelWorkingWithDepositViewModel?.BankRepository
                              where client.Deposit != null
                              where client != DataContext as BankClient<Account>
                              select client;

                        if (onliDeposit != null)
                        {
                            this.List_SortRepository.ItemsSource = onliDeposit;
                        }
                        break;

                    case 1: //Недепозитные счета

                        IEnumerable<BankClient<Account>> onliNoDeposit = from client in PanelWorkingWithDepositViewModel?.BankRepository
                                                                       where client.NoDeposit != null
                                                                       where client != DataContext as BankClient<Account>
                                                                       select client;
                        if (onliNoDeposit != null)
                        {
                            this.List_SortRepository.ItemsSource = onliNoDeposit;
                        }
                        break;

                     case 2: //Самоу себе

                        IEnumerable<BankClient<Account>> currentClient = from client in PanelWorkingWithDepositViewModel?.BankRepository
                                where client == DataContext as BankClient<Account>
                                select client;

                        if (currentClient != null)
                        {
                            List_SortRepository.ItemsSource = currentClient;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Помогает определить нужный счет клинета при переводе самому себе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocusDeposit_TextBox(object sender, RoutedEventArgs e)
        {
            if (ConditionsForListAccounts.SelectedIndex == 2)
            {
                PanelWorkingWithDepositViewModel.Recipient = (DataContext as BankClient<Account>)?.NoDeposit; 
            }
        }
        /// <summary>
        /// Помогает определить нужный счет клинета при переводе самому себе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocusNoDeposit_TextBox(object sender, RoutedEventArgs e)
        {
            if (ConditionsForListAccounts.SelectedIndex == 2)
            {
                PanelWorkingWithDepositViewModel.Recipient = (DataContext as BankClient<Account>)?.Deposit;
            }
        }

        /// <summary>
        /// Обнуляет список клиентов получателей платежа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentClientChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.List_SortRepository.ItemsSource = null;
            this.ConditionsForListAccounts.SelectedIndex = -1;
        }
    }
}
