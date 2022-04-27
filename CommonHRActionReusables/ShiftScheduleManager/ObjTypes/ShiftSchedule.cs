using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.ShiftScheduleManager
{
    public class ShiftSchedule
    {

        public int SchedId { get; }

        public int EmployeeId { get; }

        public DayOfWeek DayOfWeek { get; }

        public DateTime TimeStart { get; }

        public DateTime TimeEnd { get; }

        internal ShiftSchedule(int timeId, int empId, DayOfWeek dayOfWeek,
            DateTime timeStart, DateTime timeEnd)
        {
            SchedId = timeId;
            EmployeeId = empId;
            DayOfWeek = dayOfWeek;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
        }

        public static string GetDateTimeHHString(DateTime dateTime)
        {
            return String.Format("{0:D2}", dateTime.Hour);
        }

        public static string GetDateTimeMinMinString(DateTime dateTime)
        {
            return String.Format("{0:D2}", dateTime.Minute);
        }


        //

        public class Builder
        {

            public int EmployeeId { set; get; }

            public DayOfWeek DayOfWeek { set; get; }


            private DateTime timeStart;

            public DateTime TimeStart { 
                set
                {
                    var cleanedVal = new DateTime(2000, 1, 1, value.Hour, value.Minute, 0);
                    

                    timeStart = cleanedVal;
                }
                get
                {
                    return timeStart;
                }
            }

            private DateTime timeEnd;
            public DateTime TimeEnd
            {
                set
                {
                    var cleanedVal = new DateTime(2000, 1, 1, value.Hour, value.Minute, 0);


                    timeEnd = cleanedVal;
                }
                get
                {
                    return timeEnd;
                }
            }

            public ShiftSchedule build(int timeId)
            {
                return new ShiftSchedule(timeId, EmployeeId, DayOfWeek, TimeStart, TimeEnd);
            }

        }

    }
}
