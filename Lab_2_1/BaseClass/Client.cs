using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2_1.BaseClass
{
    internal class Client
    {
        public List<BankAccount> BankAccounts { get; private set; } = new();

        public void AddBankAccount(BankAccount bankAccount) => BankAccounts.Add(bankAccount);

        public double GetTotalMoney(Func<List<BankAccount>, double> calculationFunc) => calculationFunc(BankAccounts);
    }
}
