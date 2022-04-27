using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.TimeInSpanManager.Configs;
using CommonDatabaseActionReusables.AccountManager.Actions.Exceptions;
using CommonHRActionReusables.TimeInSpanManager.Exceptions;
using CommonDatabaseActionReusables.AccountManager.Actions;
using System.Data.SqlClient;

namespace CommonHRActionReusables.TimeInSpanManager.Actions
{
    public class AdvancedGetTimeInOrOutLogOfEmployeeAction : AbstractAction<TimeInSpanDatabasePathConfig>
    {

        internal AdvancedGetTimeInOrOutLogOfEmployeeAction(TimeInSpanDatabasePathConfig config) : base(config)
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
        /// <returns>A list of <see cref="TimeInOrOutLog"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.<br/><br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, descending</returns>
        public IReadOnlyList<TimeInOrOutLog> AdvancedGetTimeInOrOutLogAsList(AdvancedGetParameters adGetParameter, int employeeId)
        {

            var list = new List<TimeInOrOutLog>();

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT [{0}], [{1}], [{2}] FROM [{3}] WHERE [{4}] = @EmpId {5} {6} {7}",
                        databasePathConfig.EmployeeIdColumnName, databasePathConfig.TimeColumnName, databasePathConfig.IsStartColumnName,
                        databasePathConfig.TimeInSpanTableName,
                        databasePathConfig.EmployeeIdColumnName,
                        adGetParameter.GetSQLStatementFromOrderBy(databasePathConfig.TimeSpanIdColumnName, OrderType.DESCENDING),
                        adGetParameter.GetSQLStatementFromOffset(),
                        adGetParameter.GetSQLStatementFromFetch()
                        );

                    command.Parameters.Add(new SqlParameter("EmpId", employeeId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var dateTime = reader.GetSqlDateTime(1).Value;
                            var isStart = reader.GetSqlBoolean(2).Value;

                            list.Add(new TimeInOrOutLog(dateTime, isStart));
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
        /// <returns>A list of <see cref="TimeInOrOutLog"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.<br/><br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, descending</returns>
        public IReadOnlyList<TimeInOrOutLog> TryAdvancedGetTimeInOrOutLogAsList(AdvancedGetParameters adGetParameter, int employeeId)
        {
            try
            {
                return AdvancedGetTimeInOrOutLogAsList(adGetParameter, employeeId);
            }
            catch (Exception)
            {
                return new List<TimeInOrOutLog>();
            }
        }


    }
}
