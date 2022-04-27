using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.TimeInSpanManager.Exceptions
{
    public class TimeSpanOfEmployeeAlreadyHasStartException : ApplicationException
    {

        public int EmployeeId { get; }

        internal TimeSpanOfEmployeeAlreadyHasStartException(int employeeId)
        {
            EmployeeId = employeeId;
        }

    }
}
