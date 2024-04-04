using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Exceptions
{
    public class PassInException : SystemException
    {
        public PassInException(string message) : base(message)
        {

        }
    }
}
