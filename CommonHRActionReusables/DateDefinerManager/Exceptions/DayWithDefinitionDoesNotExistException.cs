using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.DateDefinerManager.Exceptions
{
    public class DayWithDefinitionDoesNotExistException : ApplicationException
    {

        public int DateDefinitionId { get; }


        public DayWithDefinitionDoesNotExistException(int dateDefinitionId)
        {
            DateDefinitionId = dateDefinitionId;
        }

    }
}
