using Dz7tum.enums;
using System;
using System.Collections;
namespace Dz7tum.classes
{
    public class Bankacc : IDisposable
    {
        private static uint _accountCounter;
        private readonly uint _accountNumber;
        private decimal balance;
        private AccountType type;
        private Queue _transactions = new Queue();
        private bool _disposed = false;

        public Bankacc() : this(0m, AccountType.Current)
        {
        }

        public Bankacc(decimal initialBalance) : this(initialBalance, AccountType.Current)
        {
        }

        public Bankacc(AccountType accountType) : this(0m, accountType)
        {
        }

        public Bankacc(decimal initialBalance, AccountType accountType)
        {
            balance = initialBalance;
            type = accountType;
            _accountNumber = GenerateAccountNumber();
        }

        private static uint GenerateAccountNumber()
        {
            return _accountCounter++;
        }

        public uint accountNumber
        {
            get { return _accountNumber + 1; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public AccountType Type
        {
            get { return type; }
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                balance += amount;
                BankTransaction transaction = new BankTransaction(amount);
                _transactions.Enqueue(transaction);
            }
            else
            {
                Console.WriteLine("Сумма пополнения должна быть больше нуля.");
            }
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > 0 && balance >= amount)
            {
                balance -= amount;
                BankTransaction transaction = new BankTransaction(-amount);
                _transactions.Enqueue(transaction);
                return true;
            }
            else
            {
                Console.WriteLine("Ошибка: Недостаточно средств на счете.");
                return false;
            }
        }

        public bool DepositFromOther(ref Bankacc sourceAccount, decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Сумма перевода должна быть больше нуля.");
                return false;
            }

            if (sourceAccount.balance < amount)
            {
                Console.WriteLine($"Ошибка: На счете №{sourceAccount.accountNumber} недостаточно средств.");
                return false;
            }

            sourceAccount.balance -= amount;
            sourceAccount._transactions.Enqueue(new BankTransaction(-amount));

            this.balance += amount;
            this._transactions.Enqueue(new BankTransaction(amount));

            Console.WriteLine($"Успешно переведено {amount} со счета №{sourceAccount.accountNumber} на счет №{this.accountNumber}.");
            return true;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"\nНомер счета: {accountNumber}\nТип счета: {type}\nБаланс: {balance}");
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                string fileName = $"Account_{accountNumber}_Transactions.txt";

                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine($"=== Выписка по счету №{accountNumber} (Тип: {Type}) ===");
                    while (_transactions.Count > 0)
                    {
                        BankTransaction trans = (BankTransaction)_transactions.Dequeue();
                        string operation = trans.Amount >= 0 ? "Пополнение" : "Снятие";
                        writer.WriteLine($"Дата: {trans.TransactionDate} | {operation}: {trans.Amount} руб.");
                    }
                    writer.WriteLine($"Итоговый баланс: {Balance} руб.\n");
                }

                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}