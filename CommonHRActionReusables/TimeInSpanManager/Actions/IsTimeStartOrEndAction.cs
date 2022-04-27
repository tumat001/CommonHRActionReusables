using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.TimeInSpanManager.Configs;
using CommonDatabaseActionReusables.AccountManager.Actions.Exceptions;
using CommonHRActionReusables.TimeInSpanManager.Exceptions;
using System.Data.SqlClient;

namespace CommonHRActionReusables.TimeInSpanManager.Actions
{
    public class IsTimeStartOrEndAction : AbstractAction<TimeInSpanDatabasePathConfig>
    {

        internal IsTimeStartOrEndAction(TimeInSpanDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="AccountDoesNotExistException"></exception>
        /// <returns>True if the employee id's last log is a time in, in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool IfTimeIsStartedForEmployee(int employeeId)
        {

            bool result = false;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT [{0}] FROM [{1}] WHERE [{2}] = @EmpId ORDER BY [{3}] DESC OFFSET {4} ROWS FETCH FIRST {5} ROWS ONLY",
                        databasePathConfig.IsStartColumnName, databasePathConfig.TimeInSpanTableName, databasePathConfig.EmployeeIdColumnName,
                        databasePathConfig.TimeSpanIdColumnName,
                        0, 1
                        );
                    command.Parameters.Add(new SqlParameter("EmpId", employeeId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = reader.GetSqlBoolean(0).Value;
                        }
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>True if the employee id's last log is a time in, in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool TryIfTimeIsStartedForEmployee(int employeeId)
        {
            try
            {
                return IfTimeIsStartedForEmployee(employeeId);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
