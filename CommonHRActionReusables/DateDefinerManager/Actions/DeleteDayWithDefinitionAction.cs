using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager.Configs;
using System.Data.SqlClient;
using CommonHRActionReusables.DateDefinerManager.Exceptions;

namespace CommonHRActionReusables.DateDefinerManager.Actions
{
    public class DeleteDayWithDefinitionAction : AbstractAction<DateDefinerDatabasePathConfig>
    {

        internal DeleteDayWithDefinitionAction(DateDefinerDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Deletes the day with definition with the given <paramref name="id"/>.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="DayWithDefinitionDoesNotExistException"></exception>
        /// <returns>True if the delete operation was successful. False otherwise.</returns>
        public bool DeleteDayWithDefinitionWithId(int id)
        {
            bool idExists = new DayWithDefinitionExistsAction(databasePathConfig).IfDayWithDefinitionIdExsists(id);
            if (!idExists)
            {
                throw new DayWithDefinitionDoesNotExistException(id);
            }

            //


            bool isSuccessful = false;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("DELETE FROM [{0}] WHERE [{1}] = @Id",
                        databasePathConfig.DateDefinerTableName, databasePathConfig.IdColumnName);
                    command.Parameters.Add(new SqlParameter("Id", id));

                    isSuccessful = command.ExecuteNonQuery() > 0;

                }
            }

            return isSuccessful;
        }


        /// <summary>
        /// Deletes the day with definition with the given <paramref name="id"/>.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="DayWithDefinitionDoesNotExistException"></exception>
        /// <returns>True if the delete operation was successful. False otherwise.</returns>
        public bool TryDeleteDayWithDefinitionWithId(int id)
        {
            try
            {
                return DeleteDayWithDefinitionWithId(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
