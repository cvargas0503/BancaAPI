using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.Exceptions
{
    public class DuplicateAccountException : Exception
    {
        public DuplicateAccountException(string numeroCuenta)
        : base($"An account with number '{numeroCuenta}' already exists.") { }
    }

}
