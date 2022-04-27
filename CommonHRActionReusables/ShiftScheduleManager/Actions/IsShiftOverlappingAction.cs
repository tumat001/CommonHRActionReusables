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
    public class IsShiftOverlappingAction : AbstractAction<ShiftScheduleDatabasePathConfig>
    {

        internal IsShiftOverlappingAction(ShiftScheduleDatabasePathConfig config) : base(config)
        {

        }

        //



        /// <summary>
        /// 
        /// </summary>
        /// <param name="shiftSchedule"></param>
        /// <param name="blacklistedIds">The ids to ignore when looking for overlaps. Null stands for no ids are to be blacklisted</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the <see cref="ShiftSchedule"/>'s time frame overlaps with another shift in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool IfShiftOverlapsWithExistingShifts(ShiftSchedule.Builder shiftSchedule, IList<int> blacklistedIds = null)
        {

            bool result = false;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT [{0}] FROM [{1}] WHERE [{2}] = @ShiftDayOfWeek " +
                        "AND [{9}] = @EmpIdVal " +
                        "AND ( ((DATEPART(HOUR, [{3}]) < '{4}' OR (DATEPART(HOUR, [{3}]) = '{4}' AND DATEPART(MINUTE, [{3}]) <= '{5}')) " +
                        "AND (DATEPART(HOUR, [{6}]) > '{7}' OR (DATEPART(HOUR, [{6}]) = '{7}' AND DATEPART(MINUTE, [{6}]) >= '{8}'))) " +
                        "OR " +
                        "((DATEPART(HOUR, [{10}]) < '{11}' OR (DATEPART(HOUR, [{10}]) = '{11}' AND DATEPART(MINUTE, [{10}]) <= '{12}')) " +
                        "AND (DATEPART(HOUR, [{13}]) > '{14}' OR (DATEPART(HOUR, [{13}]) = '{14}' AND DATEPART(MINUTE, [{13}]) >= '{15}'))) " +
                        "OR " +
                        "((DATEPART(HOUR, [{16}]) > '{17}' OR (DATEPART(HOUR, [{16}]) = '{17}' AND DATEPART(MINUTE, [{16}]) >= '{18}')) " +
                        "AND (DATEPART(HOUR, [{19}]) < '{20}' OR (DATEPART(HOUR, [{19}]) = '{20}' AND DATEPART(MINUTE, [{19}]) <= '{21}')) )" +
                        ") " +
                        "{22}",
                        databasePathConfig.SchedIdColumnName, databasePathConfig.TableName,
                        databasePathConfig.DayOfWeekColumnName,

                        databasePathConfig.TimeStartColumnName,
                        ShiftSchedule.GetDateTimeHHString(shiftSchedule.TimeStart), ShiftSchedule.GetDateTimeMinMinString(shiftSchedule.TimeStart),
                        databasePathConfig.TimeEndColumnName,
                        ShiftSchedule.GetDateTimeHHString(shiftSchedule.TimeStart), ShiftSchedule.GetDateTimeMinMinString(shiftSchedule.TimeStart),

                        databasePathConfig.EmployeeIdColumnName,

                        databasePathConfig.TimeStartColumnName,
                        ShiftSchedule.GetDateTimeHHString(shiftSchedule.TimeEnd), ShiftSchedule.GetDateTimeMinMinString(shiftSchedule.TimeEnd),
                        databasePathConfig.TimeEndColumnName,
                        ShiftSchedule.GetDateTimeHHString(shiftSchedule.TimeEnd), ShiftSchedule.GetDateTimeMinMinString(shiftSchedule.TimeEnd),

                        databasePathConfig.TimeStartColumnName,
                        ShiftSchedule.GetDateTimeHHString(shiftSchedule.TimeStart), ShiftSchedule.GetDateTimeMinMinString(shiftSchedule.TimeStart),
                        databasePathConfig.TimeEndColumnName,
                        ShiftSchedule.GetDateTimeHHString(shiftSchedule.TimeEnd), ShiftSchedule.GetDateTimeMinMinString(shiftSchedule.TimeEnd),

                        GetSQLFromBlacklistedIds(blacklistedIds, databasePathConfig.SchedIdColumnName)
                        );

                   
                    command.Parameters.Add(new SqlParameter("ShiftDayOfWeek", (int) shiftSchedule.DayOfWeek));
                    command.Parameters.Add(new SqlParameter("EmpIdVal", shiftSchedule.EmployeeId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = reader.HasRows;
                    }
                }
            }

            return result;
        }


        private string GetSQLFromBlacklistedIds(IList<int> blacklistedIds, string schedIdColName)
        {
            var sBuilder = new StringBuilder();

            if (blacklistedIds != null && blacklistedIds.Count != 0) 
            {
                sBuilder.Append("AND (");

                for (int i = 0; i < blacklistedIds.Count; i++)
                {
                    sBuilder.Append(string.Format("[{0}] != {1}", schedIdColName, blacklistedIds[i]));

                    if (i != blacklistedIds.Count - 1)
                    {
                        sBuilder.Append(" OR ");
                    }
                }

                sBuilder.Append(")");
            }

            return sBuilder.ToString();
        }

    }
}
