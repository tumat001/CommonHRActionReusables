using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.AttendanceHistoryManager 
{

    public class AttendanceHistory
    {

        public int EmployeeId { get; }

        public DateTime DateTimeOfStart { get; }
    
        public int TotalMinutes { get; }

        public int MinutesPresent { get; }

        internal AttendanceHistory(int empId, DateTime dateTimeOfStart,
            int totalMins, int minsPresent)
        {
            EmployeeId = empId;
            DateTimeOfStart = dateTimeOfStart;
            TotalMinutes = totalMins;
            MinutesPresent = minsPresent;
        }


        public class Builder
        {
            public int EmployeeId { set; get; }

            public DateTime DateTimeOfStart { set; get; }

            public int TotalMinutes { set; get; }

            public int MinutesPresent { set; get; }


            public AttendanceHistory build()
            {
                return new AttendanceHistory(EmployeeId, DateTimeOfStart, TotalMinutes, MinutesPresent);
            }

        }

    }
}
