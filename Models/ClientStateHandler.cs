using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Models
{
    public class ClientStateHandler : EventArgs, INotifyPropertyChanged
    {
        /// <summary>
        /// Дата и время операции
        /// </summary>
        public DateTime Date { get; private set; }

       
        /// <summary>
        /// Кто произмел изменение
        /// </summary>
        public string WhoChangedIt { get; private set; }

        private string whatChanges;
        /// <summary>
        /// Какое поле измненилось
        /// </summary>
        public string WhatChanges
        {
            get { return this.whatChanges; }
            private set
            {
                this.whatChanges = value;
                OnPropertyChanged(nameof(WhatChanges));
            }
        }

        public ClientStateHandler(DateTime date, string whoChangedIt, string whatChanges)
        {
            this.Date = date;
            this.WhoChangedIt = whoChangedIt;
            this.WhatChanges = whatChanges;
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
