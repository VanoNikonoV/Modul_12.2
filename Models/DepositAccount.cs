﻿using Modul_13.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Models
{
    /// <summary>
    /// Счет для начисления процентов
    /// </summary>
    public class DepositAccount : Account, IAccount<Account>
    {
        public DepositAccount(decimal initialBalance, decimal minimumBalance) : base(initialBalance, minimumBalance) { }

        public Account GetAccount => this;

        public override void PerformMonthEndTransactions()
        {
            if (Balance > 500m)
            {
                decimal interest = Balance * 0.05m;

                MakeDeposit(interest,
                DateTime.Now,
                "Начислены ежемесячные проценты");
            }
        }


        public Account TopUpAccount(decimal sum)
        {
            this.MakeDeposit(sum, DateTime.Now, $"Пополенение: {sum}");

            return this;
        }
    }
}
