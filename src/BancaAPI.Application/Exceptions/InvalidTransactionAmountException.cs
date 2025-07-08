using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.Exceptions
{
    public class InvalidTransactionAmountException : Exception
    {
        public InvalidTransactionAmountException(decimal amount)
            : base($"The transaction amount must be greater than zero. Entered amount: {amount}.") { }
    }

}
