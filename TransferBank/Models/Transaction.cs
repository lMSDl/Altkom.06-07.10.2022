using System.Security.Principal;
using TransferBank.Interfaces;

namespace TransferBank.Models
{
    public class Transaction
    {
        public TransactionType Type { get; }
        public IAccount Account { get; }
        public double Amount { get; }

        public Transaction(TransactionType type, IAccount account, double amount)
        {
            Type = type;
            Account = account;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Account.AccountNumber}: {(Type == TransactionType.Debit ? "-" : "+")}{Amount}";
        }
    }
}
