using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.TimeInSpanManager.Configs;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.TimeInSpanManager.Actions;
using CommonHRActionReusables.TimeInSpanManager.Listeners;

namespace CommonHRActionReusables.TimeInSpanManager
{
    public class TimeInSpanDatabaseManagerHelper
    {

        public TimeInSpanDatabasePathConfig PathConfig { set; get; }

        private readonly IList<ITimeInAndOutListener> listeners;


        public TimeInSpanDatabaseManagerHelper(TimeInSpanDatabasePathConfig argPathConfig)
        {
            PathConfig = argPathConfig;

            listeners = new List<ITimeInAndOutListener>();
        }

        //

        #region "TimeInAndOut listener modi"

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


        #endregion


        //

        #region "If Time Is Start Or End"

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
            return new IsTimeStartOrEndAction(PathConfig).IfTimeIsStartedForEmployee(employeeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>True if the employee id's last log is a time in, in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool TryIfTimeIsStartedForEmployee(int employeeId)
        {
            return new IsTimeStartOrEndAction(PathConfig).TryIfTimeIsStartedForEmployee(employeeId);
        }

        #endregion


        #region "StartAndEnd Time in"

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
            var action = new StartAndEndTimeInAction(PathConfig);
            foreach (ITimeInAndOutListener listener in listeners)
            {
                action.AddTimeInAndOutListener(listener);
            }

            action.StartTimeSpan(employeeId, dateTimeOfStart);
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
            var action = new StartAndEndTimeInAction(PathConfig);
            foreach (ITimeInAndOutListener listener in listeners)
            {
                action.AddTimeInAndOutListener(listener);
            }

            action.TryStartTimeSpan(employeeId, dateTimeOfStart);
        }


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
            var action = new StartAndEndTimeInAction(PathConfig);
            foreach (ITimeInAndOutListener listener in listeners)
            {
                action.AddTimeInAndOutListener(listener);
            }

            action.EndTimeSpan(employeeId, dateTimeOfEnd);
        }

        /// <summary>
        /// Creates a time span log in the database, with the "is start" set to false, indicating that the time span has ended.
        /// The time span is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="dateTimeOfEnd"></param>
        public void TryEndTimeSpan(int employeeId, DateTime dateTimeOfEnd)
        {
            var action = new StartAndEndTimeInAction(PathConfig);
            foreach (ITimeInAndOutListener listener in listeners)
            {
                action.AddTimeInAndOutListener(listener);
            }

            action.TryEndTimeSpan(employeeId, dateTimeOfEnd);
        }


        #endregion


        #region "Advanced Get Time Span"

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
            return new AdvancedGetTimeInOrOutLogOfEmployeeAction(PathConfig).AdvancedGetTimeInOrOutLogAsList(adGetParameter, employeeId);
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
            return new AdvancedGetTimeInOrOutLogOfEmployeeAction(PathConfig).TryAdvancedGetTimeInOrOutLogAsList(adGetParameter, employeeId);
        }



        #endregion


        #region "Delete All time spans"

        /// <summary>
        /// Deletes all time spans.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the delete operation was successful, even if no time span was deleted.</returns>
        public bool DeleteAllTimeSpans()
    {
        return new DeleteAllTimeSpansAction(PathConfig).DeleteAllTimeSpans();
    }

    /// <summary>
    /// Deletes all time spans.<br/>
    /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
    /// </summary>
    /// <returns>True if the delete operation was successful, even if no time span was deleted.</returns>
    public bool TryDeleteAllTimeSpans()
    {
        return new DeleteAllTimeSpansAction(PathConfig).TryDeleteAllTimeSpans();
    }

    #endregion

    }
}
