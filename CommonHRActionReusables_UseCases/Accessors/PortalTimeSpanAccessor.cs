using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.TimeInSpanManager;
using CommonHRActionReusables.TimeInSpanManager.Configs;

namespace CommonHRActionReusables_UseCases.Accessors
{
    public class PortalTimeSpanAccessor
    {

        //Change these values upon changing database
        const string DATABASE_CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\CommonHRActionReusables\CommonHRActionReusables_UseCases\Databases\EmployeeDatabase.mdf;Integrated Security=True";
        const string EMPLOYEE_TIME_SPAN_TABLE_NAME = "EmployeeTimeInSpanTable";

        const string EMPLOYEE_TIME_ID_COL_NAME = "TimeSpanId";
        const string EMPLOYEE_ID_COL_NAME = "EmployeeId";
        const string TIME_COL_NAME = "Time";
        const string IS_START_COL_NAME = "IsStart";

        public TimeInSpanDatabasePathConfig DatabasePathConfig { get; }

        public TimeInSpanDatabaseManagerHelper TimeInSpanManagerHelper { get; }


        public PortalTimeSpanAccessor()
        {
            DatabasePathConfig = new TimeInSpanDatabasePathConfig(DATABASE_CONN_STRING, EMPLOYEE_TIME_ID_COL_NAME,
                EMPLOYEE_ID_COL_NAME, TIME_COL_NAME, IS_START_COL_NAME, EMPLOYEE_TIME_SPAN_TABLE_NAME);

            TimeInSpanManagerHelper = new TimeInSpanDatabaseManagerHelper(DatabasePathConfig);
        }

    }
}
