﻿using Modul_13.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modul_13.Models
{
    /// <summary>
    /// 
    /// Счет для начисления процентов
    /// </summary>
    public class DepositAccount : Account, ICovAccount<DepositAccount>, IContrAccount<Account>
    {
        
        public DepositAccount(decimal initialBalance) : base(initialBalance) { }

        public DepositAccount MakeDeposit(decimal amount)
        {
            this.Balance += amount;

            return this;
        }

        public void MakeWithdrawal(Account client, decimal amount)
        {
            if (client.Balance >= amount)
            {
                client.Balance -= amount;
            }
            else MessageBox.Show("Недостаточно средств");
        }

        public override void PerformMonthEndTransactions()
        {
            if (Balance > 500m)
            {
                this.Balance *= 0.05m;
            }
        } 
    }
}
