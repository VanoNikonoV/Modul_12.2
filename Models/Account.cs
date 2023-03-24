using System;
using System.Collections.Generic;
using System.ComponentModel;
using Modul_13.Interfases;

namespace Modul_13.Models
{
    //https://learn.microsoft.com/ru-ru/dotnet/csharp/fundamentals/tutorials/oop

    //    Сделайте так, чтобы в журнале отражались время операции и имя совершившего операцию(консультанта или менеджера) при:
    // •	открытии счёта;
    // •	закрытии счёта; 
    // •	пополнении счёта;
    // •	переводе между счетами;
    // •	изменении данных клиента.


    public class Account : INotifyPropertyChanged
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public string Number { get; }


        private decimal balance;
        /// <summary>
        /// Баланс
        /// </summary>
        public decimal Balance 
        {
            get => balance;

            set { 
                if (balance == value) return; 
                this.balance = value;
                OnPropertyChanged(nameof(Balance));
            }            
        }

        /// <summary>
        /// Номер счета
        /// </summary>
        private static int accountNumberSeed = 0;

        /// <summary>
        /// Конструтор Account 
        /// </summary>
        /// <param name="initialBalance">Начальный баланс при открытии счета</param>
        public Account(decimal initialBalance)
        {
            this.Number = accountNumberSeed.ToString();
            accountNumberSeed++;

            Balance = initialBalance;
        }

        public Account()
        {
            this.Number = accountNumberSeed.ToString();
            accountNumberSeed++;
        }

        /// <summary>
        /// Операция с денежными средствами в конце каждого месяца
        /// </summary>
        public virtual void PerformMonthEndTransactions() { }


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}
