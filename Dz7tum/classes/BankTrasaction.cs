namespace Dz7tum.classes
{
        public class BankTransaction
        {
            public readonly DateTime TransactionDate;
            public readonly decimal Amount;

            public BankTransaction(decimal amount)
            {
                TransactionDate = DateTime.Now;
                Amount = amount;
            }
        }
    }

