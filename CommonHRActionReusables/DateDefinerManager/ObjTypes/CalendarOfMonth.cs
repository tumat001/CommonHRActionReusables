using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CommonHRActionReusables.DateDefinerManager
{
    public class CalendarOfMonth
    {

        public IReadOnlyDictionary<int, DayTypes> dayToDayTypeMap;
        public int month;
        public int year;


        public CalendarOfMonth(DateTime dateTimeOfMonth)
        {
            var internal_dayToDayTypeMap = new Dictionary<int, DayTypes>();
            var calendar = new GregorianCalendar();

            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            for (int i = 1; i < calendar.GetDaysInMonth(dateTimeOfMonth.Year, dateTimeOfMonth.Month, myCal.GetEra(dateTimeOfMonth)) + 1; i++)
            {
                internal_dayToDayTypeMap.Add(i, DayTypes.REGULAR);
            }


            dayToDayTypeMap = internal_dayToDayTypeMap;

            //

            month = dateTimeOfMonth.Month;
            year = dateTimeOfMonth.Year;
        }

        //make another constructor for when reading from database (accepts map)

    }
}
