using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.PathConfig;

namespace CommonHRActionReusables.DateDefinerManager.Configs
{
    public class DateDefinerDatabasePathConfig : DatabasePathConfig
    {

        public string DateDefinerTableName { get; }

        public string IdColumnName { get; }
        public string DateTimeOfDayColumnName { get; }
        public string DayTitleColumnName { get; }
        public string DayDescriptionColumnName { get; }
        public string DayTypeColumnName { get; }
        public string RepeatPerYearColumnName { get; }


        public DateDefinerDatabasePathConfig(string connString, string idColumnName, string dateTimeOfDateColumnName,
            string dayTitleColumnName, string dayDescriptionColumnName, string dayTypeColumnName,
            string repeatPerYearColumnName, string dateDefinerTableName) : base(connString)
        {
            IdColumnName = idColumnName;
            DateTimeOfDayColumnName = dateTimeOfDateColumnName;
            DayTitleColumnName = dayTitleColumnName;
            DayDescriptionColumnName = dayDescriptionColumnName;
            DayTypeColumnName = dayTypeColumnName;
            RepeatPerYearColumnName = repeatPerYearColumnName;

            DateDefinerTableName = dateDefinerTableName;
        }

    }
}
