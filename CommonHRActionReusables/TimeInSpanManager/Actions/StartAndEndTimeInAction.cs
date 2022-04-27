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
using CommonHRActionReusables.TimeInSpanManager.Listeners;

namespace CommonHRActionReusables.TimeInSpanManager.Actions
{
    public class StartAndEndTimeInAction : AbstractAction<TimeInSpanDatabasePathConfig>
    {

        private readonly IList<ITimeInAndOutListener> listeners;


        internal StartAndEndTimeInAction(TimeInSpanDatabasePathConfig config) : base(config)
        {
            listeners = new List<ITimeInAndOutListener>();
        }

        //

        public void AddTimeInAndOutListener(ITimeInAndOutListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveTimeInAndOutListener(ITimeInAndOutListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public bool HasTimeInAndOutListener(ITimeInAndOutListener listener)
        {
            return listeners.Contains(listener);
        }



        //

        /// <summary>
        /// Creates a time span log in the database, with the "is start" set to true, indicating that the time span has started.
        /// The time span is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="dateTimeOfStart"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="TimeSpanOfEmployeeAlreadyHasStartException"></exception>
        public void StartTimeSpan(int employeeId, DateTime dateTimeOfStart)
        {
            var isEmployeeAlreadyTimeStarted = new IsTimeStartOrEndAction(databasePathConfig).IfTimeIsStartedForEmployee(employeeId);
            if (isEmployeeAlreadyTimeStarted)
            {
                throw new TimeSpanOfEmployeeAlreadyHasStartException(employeeId);
            }

            //

            int timeSpanId;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("INSERT INTO [{0}] ({1}, {2}, {3}) VALUES (@EmpId, @TimeStart, @IsStart); SELECT SCOPE_IDENTITY()",
                        databasePathConfig.TimeInSpanTableName,
                        databasePathConfig.EmployeeIdColumnName, databasePathConfig.TimeColumnName,
                        databasePathConfig.IsStartColumnName
                        );

                    command.Parameters.Add(new SqlParameter("EmpId", employeeId));
                    command.Parameters.Add(new SqlParameter("TimeStart", dateTimeOfStart));
                    command.Parameters.Add(new SqlParameter("IsStart", true));


                    timeSpanId = int.Parse(command.ExecuteScalar().ToString());
                }
            }

            AlertTimeInListeners(employeeId, dateTimeOfStart);

        }

        private void AlertTimeInListeners(int empId, DateTime dateTime)
        {
            foreach(ITimeInAndOutListener listener in listeners)
            {
                listener.TimedIn(empId, dateTime);
            }
        }


        /// <summary>
        /// Creates a time span log in the database, with the "is start" set to true, indicating that the time span has started.
        /// The time span is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="dateTimeOfStart"></param>
        /// <exception cref="InvalidCastException"></exception>
        public void TryStartTimeSpan(int employeeId, DateTime dateTimeOfStart)
        {
            try
            {
                StartTimeSpan(employeeId, dateTimeOfStart);
            }
            catch (Exception)
            {

            }
        }


        //

        /// <summary>
        /// Creates a time span log in the database, with the "is start" set to false, indicating that the time span has ended.
        /// The time span is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="dateTimeOfEnd"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="TimeSpanOfEmployeeAlreadyHasEndException"></exception>
        public void EndTimeSpan(int employeeId, DateTime dateTimeOfEnd)
        {
            var isEmployeeAlreadyTimeStarted = new IsTimeStartOrEndAction(databasePathConfig).IfTimeIsStartedForEmployee(employeeId);
            if (!isEmployeeAlreadyTimeStarted)
            {
                throw new TimeSpanOfEmployeeAlreadyHasEndException(employeeId);
            }

            //

            int timeSpanId;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("INSERT INTO [{0}] ({1}, {2}, {3}) VALUES (@EmpId, @TimeEnd, @IsStart); SELECT SCOPE_IDENTITY()",
                        databasePathConfig.TimeInSpanTableName,
                        databasePathConfig.EmployeeIdColumnName, databasePathConfig.TimeColumnName,
                        databasePathConfig.IsStartColumnName
                        );

                    command.Parameters.Add(new SqlParameter("EmpId", employeeId));
                    command.Parameters.Add(new SqlParameter("TimeEnd", dateTimeOfEnd));
                    command.Parameters.Add(new SqlParameter("IsStart", false));


                    timeSpanId = int.Parse(command.ExecuteScalar().ToString());
                }
            }

            AlertTimeOutListeners(employeeId, dateTimeOfEnd);
        }

        private void AlertTimeOutListeners(int empId, DateTime dateTime)
        {
            foreach (ITimeInAndOutListener listener in listeners)
            {
                listener.TimedOut(empId, dateTime);
            }
        }

        /// <summary>
        /// Creates a time span log in the database, with the "is start" set to false, indicating that the time span has ended.
        /// The time span is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="dateTimeOfEnd"></param>
        public void TryEndTimeSpan(int employeeId, DateTime dateTimeOfEnd)
        {
            try
            {
                EndTimeSpan(employeeId, dateTimeOfEnd);
            }
            catch (Exception)
            {

            }
        }


    }
}
