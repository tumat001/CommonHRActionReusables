using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.AttendanceHistoryManager
{
    public class AdvancedGetAttendanceHistoryParameters
    {


        public DateTime? DateTimeLowerRange { set; get; }

        public DateTime? DateTimeUpperRange { set; get; }


        public AdvancedGetAttendanceHistoryParameters()
        {

        }

        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeInQuestion"></param>
        /// <returns>True if the <paramref name="dateTimeInQuestion"/> lies between <see cref="DateTimeLowerRange"/> and <see cref="DateTimeUpperRange"/>, inclusive.</returns>
        public bool IsDateWithinRange(DateTime dateTimeInQuestion)
        {
            if (DateTimeLowerRange.HasValue & DateTimeUpperRange.HasValue)
            {
                return dateTimeInQuestion.Ticks >= DateTimeLowerRange.Value.Ticks && dateTimeInQuestion.Ticks <= DateTimeUpperRange.Value.Ticks;
            }
            else if (DateTimeLowerRange.HasValue)
            {
                return dateTimeInQuestion.Ticks >= DateTimeLowerRange.Value.Ticks;
            }
            else if (DateTimeUpperRange.HasValue)
            {
                return dateTimeInQuestion.Ticks <= DateTimeUpperRange.Value.Ticks;
            }
            else
            {
                return true;
            }
        }


        public string GetDateRangeAsQueryString(string dateColumnName)
        {

            if (DateTimeLowerRange.HasValue && DateTimeUpperRange.HasValue)
            {
                return String.Format("([{0}] >= '{1}' AND [{0}] <= '{2}')", dateColumnName,
                    GetDateTimeAsYYYYMMDDString(DateTimeLowerRange.Value),
                    GetDateTimeAsYYYYMMDDString(DateTimeUpperRange.Value.AddDays(1))
                    );
            }
            else if (DateTimeLowerRange.HasValue)
            {
                return String.Format("([{0}] >= '{1}')", dateColumnName,
                    GetDateTimeAsYYYYMMDDString(DateTimeLowerRange.Value)
                    );
            }
            else if (DateTimeUpperRange.HasValue)
            {
                return String.Format("([{0}] <= '{1}')", dateColumnName,
                    GetDateTimeAsYYYYMMDDString(DateTimeUpperRange.Value.AddDays(1))
                    );
            }
            else
            {
                return "";
            }

        }

        public string GetDateTimeAsYYYYMMDDString(DateTime dateTime)
        {
            return String.Format("{0:D4}{1:D2}{2:D2}", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public string GetDateTimeMMString(DateTime dateTime)
        {
            return String.Format("{0:D2}", dateTime.Month);
        }

        public string GetDateTimeDDString(DateTime dateTime)
        {
            return String.Format("{0:D2}", dateTime.Day);
        }

    }
}
