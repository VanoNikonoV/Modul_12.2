using Modul_13.Models;
using System;

namespace Modul_13.Interfases
{
    // Интерфейс с параметром ковариантного типа позволяет своим методам
    // возвращать аргументы производных типов, степень наследования у которых больше, чем у параметра типа.
    public interface ICovAccount<out T>
    {
        T MakeDeposit(decimal amount);
    }
    //Интерфейс с параметром контравариантного типа позволяет своим методам принимать
    //аргументы производных типов, степень наследования у которых меньше, чем у параметра типа интерфейса.
    public interface IContrAccount<in K>
    {
        void MakeWithdrawal(K client, decimal amount);
    }
    // Используя ковариантный интерфейс, реализуйте методы пополнения счёта по соответствующему типу.
    //public interface IAccount<out T> where T : Account
    //{
    //    T TopUpAccount(decimal sum);
    //}
}
