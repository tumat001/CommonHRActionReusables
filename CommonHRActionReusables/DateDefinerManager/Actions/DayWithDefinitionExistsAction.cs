using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager.Configs;
using System.Data.SqlClient;

namespace CommonHRActionReusables.DateDefinerManager.Actions
{
    public class DayWithDefinitionExistsAction : AbstractAction<DateDefinerDatabasePathConfig>
    {

        internal DayWithDefinitionExistsAction(DateDefinerDatabasePathConfig config) : base(config)
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
        /// <returns>True if the <see cref="DayWithDefinition"/> id exists in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool IfDayWithDefinitionIdExsists(int id)
        {

            bool result = false;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT [{0}] FROM [{1}] WHERE [{0}] = @TargetId",
                        databasePathConfig.IdColumnName, databasePathConfig.DateDefinerTableName);
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
        /// <returns>True if the <see cref="DayWithDefinition"/> id exists in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool TryIfDayWithDefinitionIdExsists(int id)
        {
            try
            {
                return IfDayWithDefinitionIdExsists(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
