using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string accountNumber, decimal balance, decimal withdrawal)
            : base($"Account '{accountNumber}' has insufficient funds. Current balance: {balance}, Requested withdrawal: {withdrawal}.") { }
    }

}
