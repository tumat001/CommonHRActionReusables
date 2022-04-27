using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.DateDefinerManager
{
    public enum DayTypes
    {
        REGULAR = 0,
        HOLIDAY = 1,

        ERROR = 10, //an error
    }


    public class DayType
    {

        public static DayTypes GetDayTypeFromNumber(int num)
        {
            if (num == (int) DayTypes.REGULAR)
            {
                return DayTypes.REGULAR;
            }
            else if (num == (int)DayTypes.HOLIDAY)
            {
                return DayTypes.HOLIDAY;
            }
            else
            {
                return DayTypes.ERROR;
            }
        }
    }

}
