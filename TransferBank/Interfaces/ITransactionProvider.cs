namespace TransferBank.Interfaces
{
    public interface ITransactionProvider
    {
        void To(IAccount account, double amount);
        void From(IAccount account, double amount);
    }
}