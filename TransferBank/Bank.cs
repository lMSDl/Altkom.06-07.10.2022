using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferBank.Interfaces;
using TransferBank.Models;

namespace TransferBank
{
    public class Bank
    {
        public IAccount CreateAccount()
        {
            var account = new Account();
            account.TransactionExecuted += TransactionExecuted!;
            return account;
        }

        private void TransactionExecuted(object sender, Transaction transaction)
        {
            File.AppendAllText($"{DateTime.Now.ToShortDateString()}.txt", transaction.ToString() + "\n");
        }
    }
}
