using System;
using System.ComponentModel;
using System.Windows;

namespace Modul_13.Models
{
    public class BankClient<T> : INotifyPropertyChanged where T : Account
    {
        /// <summary>
        /// Cведения о владельце счета
        /// </summary>
        public Client Owner { get; set; }

        /// <summary>
        /// Коллекция счетов
        /// </summary>
        public T[] Accounts { get; private set; } 
   
        /// <summary>
        /// Конструктор клиента банка, с возможностью завести два счета
        /// </summary>
        /// <param name="bankClient">Базованя информация о клиенте</param>
        /// <param name="deposit">Депозитный счет</param>
        /// <param name="noDeposit">Не депозитный счет</param>
        public BankClient(Client owner)
        {
            this.Owner = owner;

            this.Accounts = new T[2];
        }

        /// <summary>
        /// Открытие нового депозитного счета 
        /// </summary>
        /// <param name="selectedAccount">Выбранный тип счета</param>
        /// <param name="initialBalance">Начальный баланс при открытии счета</param>
        /// <param name="minimumBalance">Минимальный баланс при открытии счета</param>
        public void AddAccount(AccountType selectedAccount, decimal initialBalance, decimal minimumBalance)
        {
            switch (selectedAccount)
            {
                case AccountType.Deposit:

                    this.Accounts[0] = new DepositAccount(initialBalance, minimumBalance) as T;

                    OnPropertyChanged(nameof(Accounts));

                    break;
                case AccountType.NoDeposit:

                    this.Accounts[1] = new NoDepositAccount(initialBalance, minimumBalance) as T;

                    OnPropertyChanged(nameof(Accounts));

                    break;
                default:
                    MessageBox.Show("Что-то пошло не так!");
                    break;
            }
        }
        /// <summary>
        /// Закрытие депозитного счета
        /// </summary>
        public void CloseAccount(AccountType selectedAccount) 
        {
            switch (selectedAccount)
            {
                case AccountType.Deposit:

                    this.Accounts[0] = null;

                    OnPropertyChanged(nameof(Accounts));

                    break;
                case AccountType.NoDeposit:

                    this.Accounts[1] = null;

                    OnPropertyChanged(nameof(Accounts));

                    break;
                default:
                    MessageBox.Show("Что-то пошло не так!");
                    break;
            }
        }

        /// <summary>
        /// Перевод денежных средств на счет получателя
        /// </summary>
        /// <param name="recipient">Получатель платежа</param>
        /// <param name="amount">Сумма перевода</param>
        public void Transfer(BankClient<T> recipient, decimal amount)
        {
            if(recipient != this)
            {
                this.Accounts[0].MakeWithdrawal(amount, DateTime.Now, $"Списание {amount} в пользу клиента с:{recipient.Owner.FirstName}");

                recipient.Accounts[0].MakeDeposit(amount, DateTime.Now, $"Перевод {amount} от клиента с : {this.Owner.FirstName}");
            }
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
