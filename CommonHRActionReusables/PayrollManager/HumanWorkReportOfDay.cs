using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.DateDefinerManager;

namespace CommonHRActionReusables.PayrollManager
{
    public class HumanWorkReportOfDay
    {

        public readonly int regularMinutesPresent;
        public readonly int overtimeMinutesPresent;

        public readonly DayTypes typeOfDay;
        public readonly DateTime dateOfStartDay;


        public HumanWorkReportOfDay(int regularMinutesPresent, int overtimeMinutesPresent, DayTypes typeOfDay, DateTime dateOfStartDay)
        {
            this.regularMinutesPresent = regularMinutesPresent;
            this.overtimeMinutesPresent = overtimeMinutesPresent;
            this.typeOfDay = typeOfDay;
            this.dateOfStartDay = dateOfStartDay;
        }

        //



    }
}
