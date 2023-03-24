using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Models
{
    // Сделайте так, чтобы в журнале отражались время операции и имя совершившего операцию (консультанта или менеджера) при:
    //  •	открытии счёта;
    //  •	закрытии счёта; 
    //  •	пополнении счёта;
    //  •	переводе между счетами;
    //  •	изменении данных клиента

    public class AccountStateHandler: EventArgs
    {
        /// <summary>
        /// Дата и время операции
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Notes { get; private set; }

        /// <summary>
        /// Кто произмел изменение
        /// </summary>
        public string WhoChangedIt { get; private set; }

        /// <summary>
        /// Поступление/выведение денежных средств в данной операции
        /// </summary>
        public decimal Amount { get; private set; }

        public AccountStateHandler(DateTime date, string notes, string whoChangedIt, decimal amount)
        { 
            this.Date = date;
            this.Notes = notes;
            this.WhoChangedIt = whoChangedIt;   
            this.Amount = amount;
        }

        //public AccountStateHandler(DateTime date, string notes, string whoChangedIt) : this(date, notes, whoChangedIt, 0m) {  }
    }
}
