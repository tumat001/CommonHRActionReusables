using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.ShiftScheduleManager.Configs;
using System.Data.SqlClient;
using CommonHRActionReusables.ShiftScheduleManager.Exceptions;

namespace CommonHRActionReusables.ShiftScheduleManager.Actions
{
    public class DeleteShiftScheduleAction : AbstractAction<ShiftScheduleDatabasePathConfig>
    {

        internal DeleteShiftScheduleAction(ShiftScheduleDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Deletes the shift schedule with the given <paramref name="id"/>.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="ShiftScheduleDoesNotExistException"></exception>
        /// <returns>True if the delete operation was successful. False otherwise.</returns>
        public bool DeleteShiftScheduleWithId(int id)
        {
            bool idExists = new ShiftScheduleExistsAction(databasePathConfig).IfShiftScheduleIdExsists(id);
            if (!idExists)
            {
                throw new ShiftScheduleDoesNotExistException(id);
            }

            //


            bool isSuccessful = false;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("DELETE FROM [{0}] WHERE [{1}] = @Id",
                        databasePathConfig.TableName, databasePathConfig.SchedIdColumnName);
                    command.Parameters.Add(new SqlParameter("Id", id));

                    isSuccessful = command.ExecuteNonQuery() > 0;

                }
            }

            return isSuccessful;
        }

        //

        /// <summary>
        /// Deletes the shift schedule with the given <paramref name="id"/>.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the delete operation was successful. False otherwise.</returns>
        public bool TryDeleteShiftScheduleWithId(int id)
        {
            try
            {
                return DeleteShiftScheduleWithId(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
