using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.TimeInSpanManager.Listeners
{
    public interface ITimeInAndOutListener
    {

        void TimedIn(int empId, DateTime dateTime);

        void TimedOut(int empId, DateTime dateTime);

    }
}
