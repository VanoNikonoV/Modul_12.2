using Modul_13.Models;
using System;

namespace Modul_13.Interfases
{
    // Используя ковариантный интерфейс, реализуйте методы пополнения счёта по соответствующему типу.
    public interface IAccount<out T> where T : Account
    {
        T Account { get; } 
    }
}
