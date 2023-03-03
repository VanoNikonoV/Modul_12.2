﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Models
{
    /// <summary>
    /// Счет для начисления процентов
    /// </summary>
    public class NoDepositAccount:Account
    {
        public NoDepositAccount( decimal initialBalance, decimal minimumBalance) : base(initialBalance, minimumBalance) { }

    }
}