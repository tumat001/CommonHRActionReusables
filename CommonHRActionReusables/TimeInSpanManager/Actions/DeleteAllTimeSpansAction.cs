using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.TimeInSpanManager.Configs;
using System.Data.SqlClient;

namespace CommonHRActionReusables.TimeInSpanManager.Actions
{
    public class DeleteAllTimeSpansAction : AbstractAction<TimeInSpanDatabasePathConfig>
    {

        internal DeleteAllTimeSpansAction(TimeInSpanDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Deletes all time spans.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the delete operation was successful, even if no time span was deleted.</returns>
        public bool DeleteAllTimeSpans()
        {

            bool isSuccessful = true;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("DELETE FROM [{0}]",
                        databasePathConfig.TimeInSpanTableName);

                    command.ExecuteNonQuery();

                }
            }

            return isSuccessful;

        }


        /// <summary>
        /// Deletes all time spans.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <returns>True if the delete operation was successful, even if no time span was deleted.</returns>
        public bool TryDeleteAllTimeSpans()
        {
            try
            {
                return DeleteAllTimeSpans();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
