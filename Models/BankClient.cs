using System;
using System.ComponentModel;

namespace Modul_13.Models
{
    public class BankClient<T> : INotifyPropertyChanged where T : Account
    {
        /// <summary>
        /// Cведения о владельце счета
        /// </summary>
        public Client Owner { get; set; }

        /// <summary>
        /// Депозитный счет
        /// </summary>
        public T Deposit { get; set; }

        /// <summary>
        /// Конструктор клиента банка, с возможностью завести два счета
        /// </summary>
        /// <param name="bankClient">Базованя информация о клиенте</param>
        /// <param name="deposit">Депозитный счет</param>
        /// <param name="noDeposit">Не депозитный счет</param>
        public BankClient(Client owner, T deposit = null)
        {
            this.Owner = owner;
            this.Deposit = deposit; 
        }

        /// <summary>
        /// Открытие нового депозитного счета 
        /// </summary>
        /// <param name="initialBalance">Начальный баланс при открытии счета</param>
        /// <param name="minimumBalance">Минимальный баланс при открытии счета</param>
        public void AddDeposit(decimal initialBalance, decimal minimumBalance)
        {
            this.Deposit = new DepositAccount(initialBalance, minimumBalance) as T;
            OnPropertyChanged(nameof(Deposit));
        }
        /// <summary>
        /// Закрытие депозитного счета
        /// </summary>
        public void CloseDeposit() 
        { 
           this.Deposit  = null;
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
                this.Deposit.MakeWithdrawal(amount, DateTime.Now, $"Списание {amount} в пользу клиента с:{recipient.Owner.FirstName}");

                recipient.Deposit.MakeDeposit(amount, DateTime.Now, $"Перевод {amount} от клиента с : {this.Owner.FirstName}");
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
