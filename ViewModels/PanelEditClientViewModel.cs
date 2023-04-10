using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Modul_13.ViewModels
{
    public class PanelEditClientViewModel:ViewModel
    {
        /// <summary>
        /// AccessLevel определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public MainWindow MWindow { get; } 
        /// <summary>
        /// Уровень доступа к базе данных для консультанта и менаджера, 
        /// определяется на основании выбраного параметра в элементе ComboBox "AccessLevel_ComboBox"
        /// принадлежащего MainWindow
        /// </summary>
        public int AccessLevel
        {
            get
            {
                int? index = MWindow.AccessLevel_ComboBox.SelectedIndex;

                int s = index ?? 0;

                return s;
            }
        }
        /// <summary>
        /// Список клиентов банка
        /// </summary>
        private BankRepository bankRepository;
        /// <summary>
        /// Список клиентов банка
        /// </summary>
        public BankRepository BankRepository
        {
            get => bankRepository;
            private set
            {
                Set(ref bankRepository, value, "BankRepository");
            }
        }
        /// <summary>
        /// Выбранный из общего списка  клиент для редактирования данных
        /// </summary>
        public BankClient<Account> CurrentClient 
        { 
            get => this.MWindow.DataClients.SelectedItem as BankClient<Account>; 
        }
        public Consultant Consultant { get; }
        public Meneger Meneger { get; }

        public PanelEditClientViewModel()
        {
            this.MWindow = Application.Current.MainWindow as MainWindow;

            this.Consultant = new Consultant();

            Consultant.OnEditClient += Meneger_OnEditClient;

            this.Meneger = new Meneger(); 
            
            Meneger.OnEditClient += Meneger_OnEditClient;

            this.BankRepository = MWindow.ViewModel.BankRepository;
        }

        private void Meneger_OnEditClient(InformationAboutChanges arg)
        {
            MWindow.ViewModel.BankRepository.LogClient.Add(arg);
        }

        #region Команды

        private RelayCommand<string> _editTelefonCommand = null;
        /// <summary>
        /// Команда для редактирования телефона клиента
        /// </summary>
        public RelayCommand<string> EditTelefonCommand
            => _editTelefonCommand ?? (_editTelefonCommand = new RelayCommand<string>(EditTelefon, CanEditTelefon));


        private RelayCommand<string> editNameCommand = null;
        /// <summary>
        /// Команда для редактирования имени клиента
        /// </summary>
        public RelayCommand<string> EditNameCommand =>
            editNameCommand ?? (editNameCommand = new RelayCommand<string>(EditName, CanEdit));


        private RelayCommand<string> editMiddleNameCommand = null;
        /// <summary>
        /// Команда для редактирования отчества клиента
        /// </summary>
        public RelayCommand<string> EditMiddleNameCommand =>
            editMiddleNameCommand ?? (editMiddleNameCommand = new RelayCommand<string>(EditMiddleName, CanEdit));


        private RelayCommand<string> editSecondNameCommand = null;
        /// <summary>
        /// Команда для редактирования фамилии клиента
        /// </summary>
        public RelayCommand<string> EditSecondNameCommand =>
            editSecondNameCommand ?? (editSecondNameCommand = new RelayCommand<string>(EditSecondName, CanEdit));


        private RelayCommand<string> editSeriesAndPassportNumberCommand = null;
        /// <summary>
        /// Команда для редактирования паспортных данных клиента
        /// </summary>
        public RelayCommand<string> EditSeriesAndPassportNumberCommand =>
            editSeriesAndPassportNumberCommand ?? (editSeriesAndPassportNumberCommand
            = new RelayCommand<string>(EditSeriesAndPassportNumber, CanEdit));


        private RelayCommand<string> addAccountCommand = null;
        /// <summary>
        /// Команда добавление ДЕПОЗИТНОГО счета для выбранного клиента 
        /// </summary>
        public RelayCommand<string> AddAccountCommand =>
            addAccountCommand ?? (addAccountCommand = new RelayCommand<string>(AddAccount, CanAddAccount));


        private RelayCommand<string> closeAccountCommand = null;
        /// <summary>
        /// Команда закрытия ДЕПОЗИТНОГО счета для выбранного клиента
        /// </summary>
        public RelayCommand<string> CloseAccountCommand =>
            closeAccountCommand ?? (closeAccountCommand = new RelayCommand<string>(CloseAccount, CanCloseAccount));

        #endregion

        #region Редактирование личных данных клиента
        /// <summary>
        /// Опреляет допускается ли редактировать номер телефона клиента
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true - если для данного уровня доступа доступна возможность редактировани
        /// false - если для данного уровня доступа недостукается редактировани</returns>
        private bool CanEditTelefon(string telefon)
        {
            if (telefon != null && !String.IsNullOrWhiteSpace(telefon))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Опреляет допускается ли редактировать данные клиента
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true - если для данного уровня доступа доступна возможность редактировани
        /// false - если для данного уровня доступа недостукается редактировани</returns>
        private bool CanEdit(string args)
        {
            if (AccessLevel == 1 && CurrentClient != null
                && !String.IsNullOrWhiteSpace(args)
                && args != null)
            { return true; }

            else { return false; }
        }

        /// <summary>
        /// Метод редактирования имени клиента
        /// </summary>
        /// <param name = "client" ></ param >
        private void EditName(string newName)
        {
            Client changedClient = Meneger.EditNameClient(CurrentClient.Owner, newName.Trim());

            if (changedClient.IsValid)
            {
                int index = bankRepository.IndexOf(CurrentClient);

                bankRepository.ReplaceClient(index, changedClient);
            }
        }

        /// <summary>
        /// Метод редактирование телефона клиента
        /// </summary>
        /// <param name="telefon"></param>
        private void EditTelefon(string telefon)
        {
            //string whatChanges = string.Format(CurrentClient.Owner.Telefon + @" на " + telefon.Trim());

            //Client changedClient = Consultant.EditeTelefonClient(telefon, CurrentClient.Owner);

            //CurrentClient.Owner.Telefon = telefon;

            if (CurrentClient.Owner.IsValid)
            {
                //изменения в коллекции банка, по ID клиента
                BankClient<Account> editClient = BankRepository.First(i => i.Owner.ID == CurrentClient.Owner.ID); // try

                switch (this.AccessLevel)
                {
                    case 0: //консультант

                        Client temp = Consultant.EditeTelefonClient(telefon, editClient.Owner);

                        if (temp.IsValid == false) 
                        
                        {
                            MWindow.ViewModel.ShowStatusBarText(temp.Error);
                        }

                        editClient.Owner = Consultant.EditeTelefonClient(telefon, editClient.Owner);

                       // CurrentClient.Owner = Consultant.EditeTelefonClient(telefon, editClient.Owner);

                        //editClient.Owner.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Consultant)));

                        break;

                    case 1: //менждер

                        Consultant.OnEditClient -= Meneger_OnEditClient;

                        editClient.Owner = Meneger.EditeTelefonClient(telefon, editClient.Owner);
                        
                        Consultant.OnEditClient += Meneger_OnEditClient;
                        //editClient.Owner.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Meneger)));

                        break;

                    default:
                        break;
                }
                
            }
            else
            {
                MessageBox.Show(messageBoxText: CurrentClient.Owner.Error,
                                      caption: "Ощибка в данных",
                                     MessageBoxButton.OK,
                                    icon: MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Метод редактирования отчества клиента
        /// </summary>
        /// <param name = "client" ></ param >
        private void EditMiddleName(string middleName)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditMiddleNameClient(CurrentClient.Owner, middleName.Trim());

                int index = bankRepository.IndexOf(CurrentClient);

                bankRepository.ReplaceClient(index, changedClient);
            }

        }
        /// <summary>
        /// Метод редактирование фамили клиента
        /// </summary>
        /// <param name="secondName"></param>
        private void EditSecondName(string secondName)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditSecondNameClient(CurrentClient.Owner, secondName.Trim());

                int index = bankRepository.IndexOf(CurrentClient);

                bankRepository.ReplaceClient(index, changedClient);
            }
        }
        /// <summary>
        /// Метод редактирование паспортных данных клиента
        /// </summary>
        /// <param name="passport"></param>
        private void EditSeriesAndPassportNumber(string passport)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditSeriesAndPassportNumberClient(CurrentClient.Owner, passport.Trim());

                int index = bankRepository.IndexOf(CurrentClient);

                bankRepository.ReplaceClient(index, changedClient);
            }
        }
        #endregion

        #region Методы для работы со счетами
        /// <summary>
        /// Проверка наличия открытого счета у клиента
        /// </summary>
        /// <returns>
        /// true - если у выбранного клиента открыт счет
        /// false - если счет не открыт</returns></returns>
        private bool CanCloseAccount(string selectedAccount)
        {
            switch (selectedAccount)
            {
                case "Deposit":
                   return CurrentClient?.Deposit != null ? true : false;
                case "NoDeposit":
                   return CurrentClient?.NoDeposit != null ? true : false;

                    default: return false;
            }   
        }
        /// <summary>
        /// Выполняет поиск клиента и в случаи совпадения удаляет счет
        /// </summary>
        private void CloseAccount(string selectedAccount)
        {
            if (CurrentClient != null)
            {
                BankClient<Account> editClient = bankRepository.First(i => i.Owner.ID == CurrentClient.Owner.ID);

                switch (selectedAccount)
                {
                    case "Deposit":

                        CurrentClient.CloseAccount(AccountType.Deposit); //удаление в списке для консультанты

                        editClient.CloseAccount(AccountType.Deposit);  //удаление в общем списке

                        break;

                    case "NoDeposit":

                        CurrentClient.CloseAccount(AccountType.NoDeposit);

                        editClient.CloseAccount(AccountType.NoDeposit);

                        break;
                }
            }
            else MWindow.ViewModel.ShowStatusBarText("Выберите клиента");
        }

        /// <summary>
        /// Проверка наличия открытого счета у клиента
        /// </summary>
        /// <returns>false - если у выбранного клиента отрыт счет
        ///          true - если счет не открыт</returns>
        private bool CanAddAccount(string selectedAccount)
        {
            switch (selectedAccount)
            {
                case "Deposit":
                    return CurrentClient?.Deposit != null ? false : true;
                case "NoDeposit":
                    return CurrentClient?.NoDeposit != null ? false :true ;

                default: return false;
            }

        }
        /// <summary>
        /// Добавление счета для выбранного клиента
        /// </summary>
        private void AddAccount(string selectedAccount) 
        {
            if (CurrentClient != null)
            {
                BankClient<Account> editClient = bankRepository.First(i => i.Owner.ID == CurrentClient.Owner.ID);

                switch (selectedAccount)
                {
                    case "Deposit":

                        CurrentClient.AddAccount(AccountType.Deposit, 100);

                        editClient.AddAccount(AccountType.Deposit, 100);

                        break;

                    case "NoDeposit":

                        CurrentClient.AddAccount(AccountType.NoDeposit, 100);

                        editClient.AddAccount(AccountType.NoDeposit, 100);

                        break;

                }
            }
            else MWindow.ViewModel.ShowStatusBarText("Выберите клиента");
        }
        #endregion
 
    }
}
