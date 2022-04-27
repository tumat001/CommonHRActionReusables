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
    public class GetShiftScheduleAction : AbstractAction<ShiftScheduleDatabasePathConfig>
    {
        internal GetShiftScheduleAction(ShiftScheduleDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="DayWithDefinitionDoesNotExistException"></exception>
        /// <returns>A <see cref="ShiftSchedule"/> object containing information about the shift schedule with the provided <paramref name="id"/>.</returns>
        public ShiftSchedule GetShiftScheduleFromId(int id)
        {

            //

            bool idExists = new ShiftScheduleExistsAction(databasePathConfig).IfShiftScheduleIdExsists(id);
            if (!idExists)
            {
                throw new ShiftScheduleDoesNotExistException(id);
            }

            //

            var builder = new ShiftSchedule.Builder();
            ShiftSchedule shiftSched = null;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT [{1}], [{2}], [{3}], [{4}] FROM [{5}] WHERE [{0}] = @IdVal",
                        databasePathConfig.SchedIdColumnName, 
                        databasePathConfig.EmployeeIdColumnName, databasePathConfig.DayOfWeekColumnName, 
                        databasePathConfig.TimeStartColumnName, databasePathConfig.TimeEndColumnName,
                        databasePathConfig.TableName
                        );
                    command.Parameters.Add(new SqlParameter("IdVal", id));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var empId = reader.GetSqlInt32(0).Value;
                            var dayOfWeekInt = reader.GetSqlInt32(1).Value;
                            var timeOfStart = reader.GetSqlDateTime(2).Value;
                            var timeOfEnd = reader.GetSqlDateTime(3).Value;

                            builder.EmployeeId = empId;
                            builder.DayOfWeek = (DayOfWeek) dayOfWeekInt;
                            builder.TimeStart = timeOfStart;
                            builder.TimeEnd = timeOfEnd;

                            shiftSched = builder.build(id);
                        }
                    }
                }
            }


            return shiftSched;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="ShiftSchedule"/> object containing information about the shift schedule with the provided <paramref name="id"/>.</returns>
        public ShiftSchedule TryGetShiftScheduleFromId(int id)
        {
            try
            {
                return GetShiftScheduleFromId(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
