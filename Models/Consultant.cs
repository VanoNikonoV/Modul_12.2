﻿using Modul_13.Interfases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modul_13.Models
{
    public class Consultant : IClientDataMonitor
    {
        public event Action<InformationAboutChanges> OnEditClient;

        public Consultant() {  }
      
        /// <summary>
        /// Метод редактирования номера телефона
        /// </summary>
        /// <param name="client">Клиент чей номер необходимо отредактировать</param>
        /// <param name="newData">Новый номер</param>
        /// <returns>Клент с новым номером</returns>
        public Client EditeTelefonClient(string newTelefon, Client client)
        {
            OnEditClient?.Invoke(new InformationAboutChanges(DateTime.Now, this.GetType().Name, $"Замене {client.Telefon} на {newTelefon}", client.ID));

            Client changeClient = new Client(firstName: client.FirstName,
                                             middleName: client.MiddleName,
                                             secondName: client.SecondName,
                                                telefon: newTelefon,
                                seriesAndPassportNumber: client.SeriesAndPassportNumber,
                                              currentId: client.ID,
                                               dateTime: DateTime.Now,
                                              isChanged: true);

            //client.Telefon = newTelefon;

            //client.DateOfEntry = DateTime.Now;

            changeClient.IsChanged = true;

            return changeClient;
        }

        /// <summary>
        /// Возвращает коллекцию клиентов банка со скрытими данными
        /// </summary>
        /// <returns>IEnumerable<BankAccount></returns>
        public IEnumerable<BankClient<Account>> ViewClientsData(IEnumerable<BankClient<Account>> clients)
        { 
            List<BankClient<Account>> clientsForConsultant = new List<BankClient<Account>>();

            foreach (BankClient<Account> client in clients)
            {
                string concealment = ConcealmentOfSeriesAndPassportNumber(client.Owner.SeriesAndPassportNumber);

                Client temp = new Client(firstName: client.Owner.FirstName,
                                        middleName: client.Owner.MiddleName,
                                        secondName: client.Owner.SecondName,
                                           telefon: client.Owner.Telefon,
                           seriesAndPassportNumber: concealment,
                                          dateTime: client.Owner.DateOfEntry,
                                         currentId: client.Owner.ID,
                                         isChanged: client.Owner.IsChanged);

                temp.InfoChanges = client.Owner.InfoChanges;

                temp.IsChanged = client.Owner.IsChanged;

                BankClient<Account> clone = new BankClient<Account>(temp);

                clone.Deposit = client.Deposit;

                clone.NoDeposit = client.NoDeposit;

                clientsForConsultant.Add(clone); 
            }

            return clientsForConsultant; //clientsForConsultant;
        }

        /// <summary>
        /// Сокрыте паспортных данных клиента
        /// </summary>
        /// <param name="number">Паспорные данные</param>
        /// <returns>Скрытые данные либо "нет данных"</returns>
        private string ConcealmentOfSeriesAndPassportNumber(string number)
        {
            if (number.Length > 0 && number != null && number != String.Empty)
            {
                string data = number;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < number.Length; i++)
                {
                    if (data[i] != ' ')
                    {
                        sb.Append('*');
                    }
                    else sb.Append(data[i]);
                }
                return sb.ToString();
            }

            else return "нет данных";
        }
    }

    
}
