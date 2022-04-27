using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager.Configs;
using CommonHRActionReusables.DateDefinerManager.Exceptions;
using System.Data.SqlClient;
using CommonDatabaseActionReusables.GeneralUtilities.TypeUtilities;

namespace CommonHRActionReusables.DateDefinerManager.Actions
{
    public class GetDayWithDefinitionAction : AbstractAction<DateDefinerDatabasePathConfig>
    {

        internal GetDayWithDefinitionAction(DateDefinerDatabasePathConfig config) : base(config)
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
        /// <exception cref="DayWithDefinitionDoesNotExistException"></exception>
        /// <returns>A <see cref="DayWithDefinition"/> object containing information about the day with the provided <paramref name="id"/>.</returns>
        public DayWithDefinition GetDayWithDefinitionFromId(int id)
        {

            //

            bool idExists = new DayWithDefinitionExistsAction(databasePathConfig).IfDayWithDefinitionIdExsists(id);
            if (!idExists)
            {
                throw new DayWithDefinitionDoesNotExistException(id);
            }

            //

            var builder = new DayWithDefinition.Builder();
            DayWithDefinition dayWithDef = null;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT [{1}], [{2}], [{3}], [{4}], [{5}] FROM [{6}] WHERE [{0}] = @IdVal",
                        databasePathConfig.IdColumnName, 
                        databasePathConfig.DateTimeOfDayColumnName, databasePathConfig.DayTitleColumnName,
                        databasePathConfig.DayDescriptionColumnName, databasePathConfig.DayTypeColumnName, 
                        databasePathConfig.RepeatPerYearColumnName,
                        databasePathConfig.DateDefinerTableName);
                    command.Parameters.Add(new SqlParameter("IdVal", id));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var date = reader.GetSqlDateTime(0).Value;
                            var dayTitle = reader.GetSqlString(1).Value;
                            var dayDescription = StringUtilities.ConvertSqlStringToByteArray(reader.GetSqlString(2));
                            var dayType = DayType.GetDayTypeFromNumber(reader.GetSqlInt32(3).Value);
                            var repeatPerYear = reader.GetSqlBoolean(4).Value;

                            builder.DateTimeOfDay = date;
                            builder.DayTitle = dayTitle;
                            builder.DayDescription = dayDescription;
                            builder.DayType = dayType;
                            builder.RepeatPerYear = repeatPerYear;

                            dayWithDef = builder.build(id);

                        }
                    }
                }
            }


            return dayWithDef;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="DayWithDefinition"/> object containing information about the day with the provided <paramref name="id"/>.</returns>
        public DayWithDefinition TryGetDayWithDefinitionFromId(int id)
        {
            try
            {
                return GetDayWithDefinitionFromId(id);
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
