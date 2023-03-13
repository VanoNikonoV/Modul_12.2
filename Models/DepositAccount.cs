using Modul_13.Interfases;
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
    public class DepositAccount : Account, ICovAccount<DepositAccount>, IContrAccount<Account>
    {
        public DepositAccount()
        {
            
        }
        public DepositAccount(decimal initialBalance) : base(initialBalance) { }

        public DepositAccount MakeDeposit(decimal amount)
        {
            this.Balance += amount;

            return this;
        }

        public void MakeWithdrawal(Account client, decimal amount)
        {
            client.Balance -= amount;         
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
