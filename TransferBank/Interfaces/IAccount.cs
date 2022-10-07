using TransferBank.Models;

namespace TransferBank.Interfaces
{
    public interface IAccount
    {
        string AccountNumber { get; }
        double Balance { get; }

        void AddTransaction(Transaction transaction);
        IEnumerable<Transaction> GetTransactions();
        IEnumerable<Transaction> FilterTransactions(TransactionType? type, IAccount account);

        Task TransferAsync(IAccount toAccount, double amount, ITransactionProvider provider);
    }
}
