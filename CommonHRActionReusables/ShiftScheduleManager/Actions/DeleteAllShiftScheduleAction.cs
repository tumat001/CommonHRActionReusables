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
    public class DeleteAllShiftScheduleAction : AbstractAction<ShiftScheduleDatabasePathConfig>
    {
        internal DeleteAllShiftScheduleAction(ShiftScheduleDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Deletes all shift schedules.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the delete operation was successful, even if nothing was deleted.</returns>
        public bool DeleteAllShiftSchedules()
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
        /// Deletes all shift schedules.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <returns>True if the delete operation was successful, even if nothing was deleted.</returns>public bool TryDeleteAllDayWithDefinitions()
        public bool TryDeleteAllShiftSchedules()
        {
            try
            {
                return DeleteAllShiftSchedules();
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
