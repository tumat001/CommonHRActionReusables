using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager.Configs;
using CommonHRActionReusables.DateDefinerManager.Exceptions;
using System.Data.SqlClient;


namespace CommonHRActionReusables.DateDefinerManager.Actions
{
    public class DeleteAllDayWithDefinitionAction : AbstractAction<DateDefinerDatabasePathConfig>
    {

        internal DeleteAllDayWithDefinitionAction(DateDefinerDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Deletes all day with definitions.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the delete operation was successful, even if no day with definition was deleted.</returns>
        public bool DeleteAllDayWithDefinitions()
        {

            bool isSuccessful = true;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("DELETE FROM [{0}]",
                        databasePathConfig.DateDefinerTableName);

                    command.ExecuteNonQuery();

                }
            }

            return isSuccessful;
        }


        /// <summary>
        /// Deletes all day with definitions.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <returns>True if the delete operation was successful, even if no day with definition was deleted.</returns>
        public bool TryDeleteAllDayWithDefinitions()
        {
            try
            {
                return DeleteAllDayWithDefinitions();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
