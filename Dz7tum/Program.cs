using Dz7tum.classes;
using Dz7tum.enums;
using System;
using System.IO;
using System.Runtime.CompilerServices;
namespace Dz7tum
{

    internal class Program
    {
     static void Ex1()
    {
            Bankacc acc1 = new Bankacc();
            Bankacc acc2 = new Bankacc(5000m);
            Bankacc acc3 = new Bankacc(AccountType.Deposit);

            acc1.PrintInfo();
            acc2.PrintInfo();
            acc3.PrintInfo();

            using (Bankacc acc4 = new Bankacc(10000m, AccountType.Savings))
            {
                acc4.Deposit(2500m);
                acc4.Withdraw(4000m);
                acc4.Deposit(1500m);
                acc4.PrintInfo();
            } 
            acc1.Dispose();
            acc2.Dispose();
            acc3.Dispose();
            Console.WriteLine("Операции завершены. Файлы с выписками сохранены");
            Console.ReadKey();
        }
        static void Ex2()
        {
            Song mySong = new Song();
            mySong.PrintInfo();

            Song song1 = new Song("Castelammarese War", "BuntaSparks");
            Song song2 = new Song("Ich will", "Rammstein", song1);

            song1.PrintInfo();
            song2.PrintInfo();
        }
        
    
        static void Main(string[] args)
        {
            Console.WriteLine("Упражнения 9.1,9.2,9.3");
            Ex1();
            Console.ReadKey();

            Console.WriteLine("Домашнее задание 9.1");
            Ex2();
            Console.ReadKey();
        }
    }
}