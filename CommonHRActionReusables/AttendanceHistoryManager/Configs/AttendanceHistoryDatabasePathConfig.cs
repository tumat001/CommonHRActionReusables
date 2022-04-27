using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.PathConfig;

namespace CommonHRActionReusables.AttendanceHistoryManager.Configs
{
    public class AttendanceHistoryDatabasePathConfig : DatabasePathConfig
    {

        public string TableName { get; }

        public string EmployeeIdColName { get; }
        public string DateTimeOfStartColName { get; }
        public string TotalMinsColName { get; }
        public string MinutesPresentColName { get; }


        public AttendanceHistoryDatabasePathConfig(string connString,
            string empIdColName, string dateTimeOfStartColName,
            string totalMinsColName, string minutesPresentColName,
            string tableName) : base(connString)
        {
            EmployeeIdColName = empIdColName;
            DateTimeOfStartColName = dateTimeOfStartColName;
            TotalMinsColName = totalMinsColName;
            MinutesPresentColName = totalMinsColName;

            TableName = tableName;
        }


    }
}
