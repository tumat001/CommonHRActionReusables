using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.TimeInSpanManager
{
    public class TimeInOrOutLog
    {

        public readonly DateTime DateTimeOfLog;

        public readonly bool IsTimeIn;

        public readonly bool IsTimeOut;


        public TimeInOrOutLog(DateTime timeLog, bool isTimeIn)
        {
            DateTimeOfLog = timeLog;
            IsTimeIn = isTimeIn;
            IsTimeOut = !isTimeIn;
        }

    }
}
