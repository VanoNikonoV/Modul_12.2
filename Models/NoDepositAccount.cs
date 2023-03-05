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
    public class NoDepositAccount:Account, IAccount<NoDepositAccount>
    {
        public NoDepositAccount( decimal initialBalance, decimal minimumBalance) : base(initialBalance, minimumBalance) { }

        public NoDepositAccount TopUp(decimal sum)
        {
            this.MakeDeposit(sum, DateTime.Now, $"Пополенение: {sum}");

            return this;
        }
    }
}
