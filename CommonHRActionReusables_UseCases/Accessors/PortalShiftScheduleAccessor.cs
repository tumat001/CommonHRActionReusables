using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.ShiftScheduleManager;
using CommonHRActionReusables.ShiftScheduleManager.Configs;

namespace CommonHRActionReusables_UseCases.Accessors
{
    public class PortalShiftScheduleAccessor
    {


        //Change these values upon changing database
        const string DATABASE_CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\CommonHRActionReusables\CommonHRActionReusables_UseCases\Databases\EmployeeDatabase.mdf;Integrated Security=True";
        const string TABLE_NAME = "EmployeeScheduleTable";

        const string SCHED_ID_COL_NAME = "SchedId";
        const string EMP_ID_COL_NAME = "EmployeeId";
        const string DAY_OF_WEEK_COL_NAME = "DayOfWeek";
        const string TIME_OF_START_COL_NAME = "TimeShiftStart";
        const string TIME_OF_END_COL_NAME = "TimeShiftEnd";



        public ShiftScheduleDatabasePathConfig DatabasePathConfig { get; }

        public ShiftScheduleDatabaseManagerHelper ShiftScheduleManagerHelper { get; }


        public PortalShiftScheduleAccessor()
        {
            DatabasePathConfig = new ShiftScheduleDatabasePathConfig(DATABASE_CONN_STRING,
                SCHED_ID_COL_NAME, EMP_ID_COL_NAME,
                TIME_OF_START_COL_NAME, TIME_OF_END_COL_NAME,
                DAY_OF_WEEK_COL_NAME, TABLE_NAME
                );

            ShiftScheduleManagerHelper = new ShiftScheduleDatabaseManagerHelper(DatabasePathConfig);
        }

    }
}
