using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.ShiftScheduleManager.Configs;
using CommonHRActionReusables.ShiftScheduleManager;
using System.Data.SqlClient;

namespace CommonHRActionReusables.ShiftScheduleManager.Actions
{
    public class AdvancedGetShiftScheduleAction : AbstractAction<ShiftScheduleDatabasePathConfig>
    {

        internal AdvancedGetShiftScheduleAction(ShiftScheduleDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="adGetParameter"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns>A list of <see cref="ShiftSchedule"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.
        /// <br/>
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their day of week and time, ascending</returns>
        public IReadOnlyList<ShiftSchedule> AdvancedGetShiftScheduleAsList(int empId, AdvancedGetParameters adGetParameter)
        {

            var list = new List<ShiftSchedule>();

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    if (adGetParameter.OrderByParameters.Count == 0)
                    {
                        adGetParameter.OrderByParameters.Add(new OrderByParam(databasePathConfig.DayOfWeekColumnName, OrderType.ASCENDING));
                        adGetParameter.OrderByParameters.Add(new OrderByParam(databasePathConfig.TimeStartColumnName, OrderType.ASCENDING));
                    }

                    command.CommandText = string.Format("SELECT [{0}], [{1}], [{2}], [{3}] FROM [{4}] WHERE [{5}] = @IdVal " +
                        "{6} {7} {8}",
                        databasePathConfig.SchedIdColumnName,
                        databasePathConfig.DayOfWeekColumnName, databasePathConfig.TimeStartColumnName,
                        databasePathConfig.TimeEndColumnName,

                        databasePathConfig.TableName,

                        databasePathConfig.EmployeeIdColumnName,
                        adGetParameter.GetSQLStatementFromOrderBy(databasePathConfig.DayOfWeekColumnName, OrderType.ASCENDING),
                        adGetParameter.GetSQLStatementFromOffset(),
                        adGetParameter.GetSQLStatementFromFetch()
                        );
                    command.Parameters.Add(new SqlParameter("IdVal", empId));


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var builder = new ShiftSchedule.Builder();

                            var schedId = reader.GetSqlInt32(0).Value;
                            var dayOfWeekInt = reader.GetSqlInt32(1).Value;
                            var timeOfStart = reader.GetSqlDateTime(2).Value;
                            var timeOfEnd = reader.GetSqlDateTime(3).Value;

                            builder.EmployeeId = empId;
                            builder.DayOfWeek = (DayOfWeek) dayOfWeekInt;
                            builder.TimeStart = timeOfStart;
                            builder.TimeEnd = timeOfEnd;

                            var shiftSched = builder.build(schedId);
                            list.Add(shiftSched);
                        }

                    }
                }
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="adGetParameter"></param>
        /// <returns>A list of <see cref="ShiftSchedule"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.
        /// <br/>
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their day of week and time, ascending</returns>
        public IReadOnlyList<ShiftSchedule> TryAdvancedGetShiftScheduleAsList(int empId, AdvancedGetParameters adGetParameter)
        {
            try
            {
                return AdvancedGetShiftScheduleAsList(empId, adGetParameter);
            }
            catch (Exception)
            {
                return new List<ShiftSchedule>();
            }
        }

    }
}
