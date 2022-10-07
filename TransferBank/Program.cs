using TransferBank;

var bank = new Bank();

var account1 = bank.CreateAccount();
var account2 = bank.CreateAccount();
var account3 = bank.CreateAccount();


await account1.TransferAsync(account2, 100, new TransactionProvider());
await account2.TransferAsync(account3, 50, new TransactionProvider());
await account1.TransferAsync(account3, 10, new TransactionProvider());
await account3.TransferAsync(account1, 25, new TransactionProvider());

Console.ReadLine();