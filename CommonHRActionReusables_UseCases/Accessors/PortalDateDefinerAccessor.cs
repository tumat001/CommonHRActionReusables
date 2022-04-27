using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.DateDefinerManager;
using CommonHRActionReusables.DateDefinerManager.Configs;

namespace CommonHRActionReusables_UseCases.Accessors
{
    public class PortalDateDefinerAccessor
    {

        //Change these values upon changing database
        const string DATABASE_CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\CommonHRActionReusables\CommonHRActionReusables_UseCases\Databases\EmployeeDatabase.mdf;Integrated Security=True";
        const string DATE_DEFINITION_TABLE_NAME = "DateDefinitionTable";

        const string ID_COL_NAME = "Id";
        const string DATE_TIME_OF_DATE_COL_NAME = "DateTimeOfDate";
        const string DAY_TITLE_COL_NAME = "DayTitle";
        const string DAY_DESCRIPTION_COL_NAME = "DayDescription";
        const string DAY_TYPE_COL_NAME = "DayType";
        const string REPEAT_PER_YEAR_COL_NAME = "RepeatPerYear";

        public DateDefinerDatabasePathConfig DatabasePathConfig { get; }

        public DateDefinerDatabaseManagerHelper DateDefinerManagerHelper { get; }


        public PortalDateDefinerAccessor()
        {
            DatabasePathConfig = new DateDefinerDatabasePathConfig(DATABASE_CONN_STRING,
                ID_COL_NAME, DATE_TIME_OF_DATE_COL_NAME,
                DAY_TITLE_COL_NAME, DAY_DESCRIPTION_COL_NAME,
                DAY_TYPE_COL_NAME, REPEAT_PER_YEAR_COL_NAME,
                DATE_DEFINITION_TABLE_NAME);

            DateDefinerManagerHelper = new DateDefinerDatabaseManagerHelper(DatabasePathConfig);
        }

    }
}
