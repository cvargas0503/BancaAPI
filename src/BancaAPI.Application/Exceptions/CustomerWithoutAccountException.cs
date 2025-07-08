using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.Exceptions
{
    public class CustomerWithoutAccountException : Exception
    {
        public CustomerWithoutAccountException(Guid customerId)
            : base($"The customer with ID '{customerId}' has no registered accounts.") { }
    }

}
