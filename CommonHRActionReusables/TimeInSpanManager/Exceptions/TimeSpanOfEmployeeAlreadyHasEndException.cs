using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.TimeInSpanManager.Exceptions
{
    public class TimeSpanOfEmployeeAlreadyHasEndException : ApplicationException
    {

        public int EmployeeId { get; }

        internal TimeSpanOfEmployeeAlreadyHasEndException(int employeeId)
        {
            EmployeeId = employeeId;
        }


    }
}
