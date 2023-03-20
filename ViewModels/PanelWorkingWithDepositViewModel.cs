using Modul_13.Commands;
using Modul_13.Interfases;
using Modul_13.Models;
using Modul_13.ViewModels.Base;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.ViewModels
{
    public class PanelWorkingWithDepositViewModel : ViewModel
    {
        /// <summary>
        /// Ссылка на главное окно
        /// </summary>
        public MainWindow MWindow { get; }
        /// <summary>
        /// Отправитель платежа
        /// </summary>
        private BankClient<Account> sender;
        /// <summary>
        /// Отправитель платежа
        /// </summary>
        public BankClient<Account> Sender
        {
            get => this.MWindow.DataClients.SelectedItem as BankClient<Account>;
            set { Set(ref sender, value); }
        }
        /// <summary>
        /// Получатель платежа
        /// </summary>
        private Account recipient;
        /// <summary>
        /// Получатель платежа
        /// </summary>
        public Account Recipient 
        { 
            get => recipient;
            set { Set(ref recipient, value); }  
        }
        /// <summary>
        /// Список клиентов банка
        /// </summary>
        private BankRepository bankRepository;
        /// <summary>
        /// Список клиентов банка
        /// </summary>
        public BankRepository BankRepository
        {
            get => bankRepository;
            private set
            {
                Set(ref bankRepository, value, "BankRepository");
            }
        }
        

        #region UI

        public TextBox SumTransfer { get; set; }

        public TextBox SumAddDeposit_TextBox { get; set; }

        public TextBox SumAddNoDeposit_TextBox { get; set; }

        #endregion

        public PanelWorkingWithDepositViewModel()
        {
            this.MWindow =  Application.Current.MainWindow as MainWindow;

            this.BankRepository = MWindow.ViewModel.BankRepository;
        }

        #region Команды для работы с депозитным счетом

        private RelayCommand<string> makeTransfer = null;

        /// <summary>
        /// Команда для перевод между счетами
        /// </summary>
        public RelayCommand<string> MakeTransfer => makeTransfer ?? (makeTransfer = 
            new RelayCommand<string>(TransferExecuted, CanMakeTransfer));

        private RelayCommand<string> makeDepositCommand = null;
        /// <summary>
        /// Команда для пополнения счета
        /// </summary>
        public RelayCommand<string> MakeDepositCommand => makeDepositCommand ?? (makeDepositCommand = 
            new RelayCommand<string>(MakeDepositExecuted, CanMakeDeposit));

        #endregion

        #region Методы для работы со счетами

        /// <summary>
        /// Корректность исходных данных для выполнения перевода
        /// </summary>
        /// <param name="sum">Сумма перевода</param>
        /// <returns>false - если получать выбран
        ///          true - если получать не выбран</returns>
        private bool CanMakeTransfer(string selectedAccount)
        {
            if (Recipient != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Перевод денежных средств между счетами
        /// </summary>
        /// <<param name="selectedAccount">Тип счета</param>
        private void TransferExecuted(string selectedAccount) 
        {
            decimal amount;

            switch (selectedAccount)
            {
                case "Deposit":

                    if (Recipient == null) 
                    { 
                        MWindow.ViewModel.ShowStatusBarText("У вас один счет");

                        SumAddDeposit_TextBox.Text = string.Empty;

                        break; 
                    }

                    if (Decimal.TryParse(SumAddDeposit_TextBox.Text, out amount))
                    {
                        Recipient = (Recipient as ICovAccount<Account>).MakeDeposit(amount);

                        IContrAccount<DepositAccount> contr = new DepositAccount(10);

                        IContrAccount<Account> contr1 = contr;

                        contr.MakeWithdrawal(Sender.Deposit, amount);

                        (Recipient as IContrAccount<DepositAccount>).MakeWithdrawal(Sender.Deposit, amount);

                        SumAddDeposit_TextBox.Text = string.Empty;
                    }
                    else { MWindow.ViewModel.ShowStatusBarText("Нужно ввсети число"); }

                    break;

                case "NoDeposit":

                    if (Recipient == null) 
                    { 
                        MWindow.ViewModel.ShowStatusBarText("У вас один счет");

                        SumAddNoDeposit_TextBox.Text = string.Empty;

                        break; 
                    }

                    if (Decimal.TryParse(SumAddNoDeposit_TextBox.Text, out amount))
                    {
                        Recipient = (Recipient as ICovAccount<Account>).MakeDeposit(amount);

                        (Recipient as IContrAccount<Account>).MakeWithdrawal(Sender.NoDeposit, amount);

                        SumAddNoDeposit_TextBox.Text = string.Empty;

                    }
                    else { MWindow.ViewModel.ShowStatusBarText("Нужно ввсети число"); }

                    break;
            }

           
        }

        /// <summary>
        /// Возможность выполнения операции пополнение счета
        /// </summary>
        /// <param name="selectedAccount">Тип счета</param>
        /// <returns></returns>
        private bool CanMakeDeposit(string selectedAccount)
        {
            return true;
        }

        /// <summary>
        /// Пополнение счета по соответствующему типу
        /// </summary>
        /// <param name="selectedAccount">Тип счета</param>
        private void MakeDepositExecuted(string selectedAccount)
        {
            decimal amount;

            switch (selectedAccount)
            {
                case "Deposit":

                    if (Decimal.TryParse(SumAddDeposit_TextBox.Text, out amount))
                    {
                        Sender.Deposit = (Sender.Deposit as ICovAccount<Account>).MakeDeposit(amount);

                        SumAddDeposit_TextBox.Text = string.Empty;
                    }
                    else { MWindow.ViewModel.ShowStatusBarText("Нужно ввсети число"); }

                    break;

                case "NoDeposit":

                    if (Decimal.TryParse(SumAddNoDeposit_TextBox.Text, out amount))
                    {
                        Sender.NoDeposit = (Sender.NoDeposit as ICovAccount<Account>).MakeDeposit(amount);

                        SumAddNoDeposit_TextBox.Text = string.Empty;
                    }
                    else { MWindow.ViewModel.ShowStatusBarText("Нужно ввсети число"); }

                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
