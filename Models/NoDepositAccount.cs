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
    public class NoDepositAccount:Account, ICovAccount<NoDepositAccount>
    {
        public NoDepositAccount( decimal initialBalance) : base(initialBalance) { }

        public NoDepositAccount MakeDeposit(decimal amount)
        {
            this.Balance += amount;

            return this;
        }
    }
}
