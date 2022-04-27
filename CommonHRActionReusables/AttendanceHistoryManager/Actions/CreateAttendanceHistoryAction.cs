using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.AttendanceHistoryManager.Configs;
using CommonHRActionReusables.AttendanceHistoryManager;
using System.Data.SqlClient;

namespace CommonHRActionReusables.AttendanceHistoryManager.Actions
{
    public class CreateAttendanceHistoryAction : AbstractAction<AttendanceHistoryDatabasePathConfig>
    {

        internal CreateAttendanceHistoryAction(AttendanceHistoryDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Creates a <see cref="AttendanceHistory"/> in the database.
        /// The <see cref="AttendanceHistory"/> is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>The id of the created <see cref="AttendanceHistory"/></returns>
        public int CreateAttendanceHistory(AttendanceHistory.Builder builder)
        {
            
            int historyId = -1;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("INSERT INTO [{0}] ({1}, {2}, {3}, {4}) " +
                        "VALUES (@EmpId, @DateTimeOfStart, @TotalMins, @MinsPresent); SELECT SCOPE_IDENTITY()",
                        databasePathConfig.TableName,
                        databasePathConfig.EmployeeIdColName,
                        databasePathConfig.DateTimeOfStartColName, databasePathConfig.TotalMinsColName,
                        databasePathConfig.MinutesPresentColName
                    );

                    command.Parameters.Add(new SqlParameter("EmpId", builder.EmployeeId));
                    command.Parameters.Add(new SqlParameter("DateTimeOfStart", builder.DateTimeOfStart));
                    command.Parameters.Add(new SqlParameter("TotalMins", builder.TotalMinutes));
                    command.Parameters.Add(new SqlParameter("MinsPresent", builder.MinutesPresent));

                    historyId = int.Parse(command.ExecuteScalar().ToString());
                }
            }

            return historyId;
        }


        /// <summary>
        /// Creates a <see cref="AttendanceHistory"/> in the database.
        /// The <see cref="AttendanceHistory"/> is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>The id of the created <see cref="AttendanceHistory"/></returns>
        public int TryCreateAttendanceHistory(AttendanceHistory.Builder builder)
        {
            try
            {
                return CreateAttendanceHistory(builder);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
