using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.ShiftScheduleManager.Configs;
using CommonHRActionReusables.ShiftScheduleManager.Exceptions;
using CommonHRActionReusables.ShiftScheduleManager;
using System.Data.SqlClient;

namespace CommonHRActionReusables.ShiftScheduleManager.Actions
{
    public class CreateShiftScheduleAction : AbstractAction<ShiftScheduleDatabasePathConfig>
    {

        internal CreateShiftScheduleAction(ShiftScheduleDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Creates a <see cref="ShiftSchedule"/> in the database.
        /// The shift schedule is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="ShiftScheduleOverlapException"
        /// <returns>The id of the created shift schedule</returns>
        public int CreateShiftSchedule(ShiftSchedule.Builder builder)
        {
            var overlaps = new IsShiftOverlappingAction(databasePathConfig).IfShiftOverlapsWithExistingShifts(builder);
            if (overlaps)
            {
                throw new ShiftScheduleOverlapException();
            }

            //

            int shiftSchedId = -1;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("INSERT INTO [{0}] ({1}, {2}, {3}, {4})" +
                        " VALUES (@EmpId, @DayOfWeek, @TimeStart, @TimeEnd); SELECT SCOPE_IDENTITY()",
                        databasePathConfig.TableName,
                        databasePathConfig.EmployeeIdColumnName,
                        databasePathConfig.DayOfWeekColumnName, databasePathConfig.TimeStartColumnName,
                        databasePathConfig.TimeEndColumnName
                    );

                    command.Parameters.Add(new SqlParameter("EmpId", builder.EmployeeId));
                    command.Parameters.Add(new SqlParameter("DayOfWeek", (int) builder.DayOfWeek));
                    command.Parameters.Add(new SqlParameter("TimeStart", GetParamOrDbNullIfParamIsNull(builder.TimeStart)));
                    command.Parameters.Add(new SqlParameter("TimeEnd", builder.TimeEnd));

                    shiftSchedId = int.Parse(command.ExecuteScalar().ToString());
                }
            }

            return shiftSchedId;
        }


        /// <summary>
        /// Creates a <see cref="ShiftSchedule"/> in the database.
        /// The shift schedule is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="ShiftScheduleOverlapException"
        /// <returns>The id of the created shift schedule, or -1 if an exception has occurred.</returns>
        public int TryCreateShiftSchedule(ShiftSchedule.Builder builder)
        {
            try
            {
                return CreateShiftSchedule(builder);
            }
            catch (Exception)
            {
                return -1;
            }
        }

    }
}
