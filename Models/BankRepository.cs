using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace Modul_13.Models
{
    public class BankRepository: ObservableCollection<BankClient<Account>>
    {
        public BankRepository(string path)
        {
           //LoadData(path);  // Не работает явное приведение Sender.Deposit = (Sender.Deposit as ICovAccount<Account>).MakeDeposit(amount);

            GetClientsRep(50);
        }

        private void LoadData(string path)
        {
            string json = File.ReadAllText(path);

            List<BankClient<Account>> temp = JsonConvert.DeserializeObject<List<BankClient<Account>>>(json);

            foreach (BankClient<Account> client in temp)
            {
                this.Add(client);
            }
        }

        public void ReplaceClient(int index, Client editClient)
        {
            this[index].Owner = editClient;
        }

        private List<InformationAboutChanges> logClient;
        
        /// <summary>
        /// Журнал событий произходящих с клиентами
        /// </summary>
        public List<InformationAboutChanges> LogClient 
        
        { 
            get => logClient ?? (logClient = new List<InformationAboutChanges>());

            set {
                if (this.logClient == value) return;
                        
                this.logClient = value;

                OnPropertyChanged(new PropertyChangedEventArgs(nameof(LogClient)));
            }
        } 

        #region Автогенерация данных
        private void GetClientsRep(int count)
        {
            long telefon = 79020000000;
            long passport = 6650565461;

            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                telefon += i;

                passport += random.Next(1, 500);

                Client _c = new Client(firstNames[BankRepository.randomize.Next(BankRepository.firstNames.Length)],
                    middleNames[BankRepository.randomize.Next(BankRepository.middleNames.Length)],
                    secondNames[BankRepository.randomize.Next(BankRepository.secondNames.Length)],
                    telefon.ToString(),
                    passport.ToString());

                this.Add(new BankClient<Account> (_c));

                this[i].AddAccount(AccountType.Deposit, 100);
                this[i].AddAccount(AccountType.NoDeposit, 200);

            }
        }

        

        static readonly string[] firstNames;

        static readonly string[] middleNames;

        static readonly string[] secondNames;

        /// <summary>
        /// Генератор псевдослучайных чисел
        /// </summary>
        static Random randomize;

        /// <summary>
        /// Статический конструктор, в котором "хранятся"
        /// данные о именах и фамилиях баз данных firstNames и lastNames
        /// </summary>
        static BankRepository()
        {
            randomize = new Random();

            firstNames = new string[] {
                "Агата",
                "Агнес",
                "Мария",
                "Аделина",
                "Ольга",
                "Людмила",
                "Аманда",
                "Татьяна",
                "Вероника",
                "Жанна",
                "Крестина",
                "Анжела",
                "Маргарита"
            };

            middleNames = new string[]
            {
                "Ивановна",
                "Петровна",
                "Васильевна",
                "Сергеевна",
                "Дмитриевна",
                "Владимировна",
                "Александровна",
                "Тимофеевна"

            };

            secondNames = new string[]
            {
                "Иванова",
                "Петрова",
                "Васильева",
                "Кузнецова",
                "Ковалёва",
                "Попова",
                "Пономарёва",
                "Дьячкова",
                "Коновалова",
                "Соколова",
                "Лебедева",
                "Соловьёва",
                "Козлова",
                "Волкова",
                "Зайцева",
                "Ершова",
                "Карпова",
                "Щукина",
                "Виноградова",
                "Цветкова",
                "Калинина"
            };

        }
        #endregion


    }
}
