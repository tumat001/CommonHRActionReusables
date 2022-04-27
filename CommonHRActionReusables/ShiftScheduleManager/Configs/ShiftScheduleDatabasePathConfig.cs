using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.PathConfig;

namespace CommonHRActionReusables.ShiftScheduleManager.Configs
{
    public class ShiftScheduleDatabasePathConfig : DatabasePathConfig
    {

        public string TableName { get; }

        public string SchedIdColumnName { get; }
        public string EmployeeIdColumnName { get; }
        public string TimeStartColumnName { get; }
        public string TimeEndColumnName { get; }
        public string DayOfWeekColumnName { get; }


        public ShiftScheduleDatabasePathConfig(string connString,
            string schedIdColName, string employeeIdColName,
            string dateTimeStartColName, string dateTimeEndColName,
            string dayOfWeekColName,
            string tableName) : base(connString)
        {
            SchedIdColumnName = schedIdColName;
            EmployeeIdColumnName = employeeIdColName;
            TimeStartColumnName = dateTimeStartColName;
            TimeEndColumnName = dateTimeEndColName;
            DayOfWeekColumnName = dayOfWeekColName;

            TableName = tableName;
        }

    }
}
