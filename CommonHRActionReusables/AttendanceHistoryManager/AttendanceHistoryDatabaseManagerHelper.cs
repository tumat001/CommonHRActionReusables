using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.AttendanceHistoryManager.Configs;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.AttendanceHistoryManager.Actions;

namespace CommonHRActionReusables.AttendanceHistoryManager
{
    public class AttendanceHistoryDatabaseManagerHelper
    {

        public AttendanceHistoryDatabasePathConfig PathConfig { set; get; }

        public AttendanceHistoryDatabaseManagerHelper(AttendanceHistoryDatabasePathConfig argPathConfig)
        {
            PathConfig = argPathConfig;
        }

        //

        #region "Create"

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
            return new CreateAttendanceHistoryAction(PathConfig).CreateAttendanceHistory(builder);
        }

        /// <summary>
        /// Creates a <see cref="AttendanceHistory"/> in the database.
        /// The <see cref="AttendanceHistory"/> is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>The id of the created <see cref="AttendanceHistory"/></returns>
        public int TryCreateAttendanceHistory(AttendanceHistory.Builder builder)
        {
            return new CreateAttendanceHistoryAction(PathConfig).TryCreateAttendanceHistory(builder);
        }

        #endregion

        #region "Delete All"

        /// <summary>
        /// Deletes all attendance history entries.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the delete operation was successful, even if nothing was deleted.</returns>
        public bool DeleteAllAttendanceHistories()
        {
            return new DeleteAllAttendanceHistoryAction(PathConfig).DeleteAllAttendanceHistories();
        }

        /// <summary>
        /// Deletes all attendance history entries.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <returns>True if the delete operation was successful, even if nothing was deleted.</returns>
        public bool TryDeleteAllAttendanceHistories()
        {
            return new DeleteAllAttendanceHistoryAction(PathConfig).TryDeleteAllAttendanceHistories();
        }

        #endregion

        #region "Advanced Get"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="adGetParameter"></param>
        /// <param name="attendParam"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns>A list of <see cref="AttendanceHistory"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.
        /// <br/>
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, ascending</returns>
        public IReadOnlyList<AttendanceHistory> AdvancedGetAttendanceHistoryAsList(int empId, AdvancedGetParameters adGetParameter, AdvancedGetAttendanceHistoryParameters attendParam)
        {
            return new AdvancedGetAttendanceHistoryAction(PathConfig).AdvancedGetAttendanceHistoryAsList(empId, adGetParameter, attendParam);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="adGetParameter"></param>
        /// <param name="attendParam"></param>
        /// <returns>A list of <see cref="AttendanceHistory"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.
        /// <br/>
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, ascending</returns>
        public IReadOnlyList<AttendanceHistory> TryAdvancedGetAttendanceHistoryAsList(int empId, AdvancedGetParameters adGetParameter, AdvancedGetAttendanceHistoryParameters attendParam)
        {
            return new AdvancedGetAttendanceHistoryAction(PathConfig).TryAdvancedGetAttendanceHistoryAsList(empId, adGetParameter, attendParam);
        }

        #endregion


    }
}
