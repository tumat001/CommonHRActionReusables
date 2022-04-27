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
    public class DeleteAllAttendanceHistoryAction : AbstractAction<AttendanceHistoryDatabasePathConfig>
    {

        internal DeleteAllAttendanceHistoryAction(AttendanceHistoryDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Deletes all attendance history entries.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the delete operation was successful, even if nothing was deleted.</returns>
        public bool DeleteAllAttendanceHistories()
        {

            bool isSuccessful = true;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("DELETE FROM [{0}]",
                        databasePathConfig.TableName);

                    command.ExecuteNonQuery();

                }
            }

            return isSuccessful;
        }


        /// <summary>
        /// Deletes all attendance history entries.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <returns>True if the delete operation was successful, even if nothing was deleted.</returns>
        public bool TryDeleteAllAttendanceHistories()
        {
            try
            {
                return DeleteAllAttendanceHistories();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
