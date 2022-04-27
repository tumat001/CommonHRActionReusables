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
    public class ShiftScheduleExistsAction : AbstractAction<ShiftScheduleDatabasePathConfig>
    {
        internal ShiftScheduleExistsAction(ShiftScheduleDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the <see cref="ShiftSchedule"/> id exists in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool IfShiftScheduleIdExsists(int id)
        {

            bool result = false;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT [{0}] FROM [{1}] WHERE [{0}] = @TargetId",
                        databasePathConfig.SchedIdColumnName, databasePathConfig.TableName);
                    command.Parameters.Add(new SqlParameter("TargetId", id));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = reader.HasRows;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the <see cref="ShiftSchedule"/> id exists in the given parameters of <see cref="DatabasePathConfig"/>. Returns false if an exception has occurred.</returns>
        public bool TryIfShiftScheduleIdExsists(int id)
        {
            try
            {
                return IfShiftScheduleIdExsists(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
