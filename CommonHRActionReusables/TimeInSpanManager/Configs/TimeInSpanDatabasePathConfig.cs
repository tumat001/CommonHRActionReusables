using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.PathConfig;

namespace CommonHRActionReusables.TimeInSpanManager.Configs
{
    public class TimeInSpanDatabasePathConfig : DatabasePathConfig
    {

        public string TimeInSpanTableName { get; }

        public string TimeSpanIdColumnName { get; }
        public string EmployeeIdColumnName { get; }
        public string TimeColumnName { get; }
        public string IsStartColumnName { get; }


        public TimeInSpanDatabasePathConfig(string connString, string timeSpanIdColumnName, string employeeIdColumnName,
            string timeColumnName, string isStartColumnName, string timeInSpanTableName) : base(connString)
        {
            TimeSpanIdColumnName = timeSpanIdColumnName;
            EmployeeIdColumnName = employeeIdColumnName;
            TimeColumnName = timeColumnName;
            IsStartColumnName = isStartColumnName;

            TimeInSpanTableName = timeInSpanTableName;
        }


    }
}
