using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.PayrollManager
{
    public class HumanWorkReportSummary
    {

        public IReadOnlyDictionary<DateTime, HumanWorkReportOfDay> dateTimeToWorkReportOfDayMap;

        public HumanWorkReportSummary(IList<HumanWorkReportOfDay> listOfHumanWorkReportDays)
        {
            var map = new Dictionary<DateTime, HumanWorkReportOfDay>();

            foreach (HumanWorkReportOfDay humanWorkReportDay in listOfHumanWorkReportDays)
            {
                map.Add(humanWorkReportDay.dateOfStartDay, humanWorkReportDay);
            }

            dateTimeToWorkReportOfDayMap = map;
        }


    }
}
