using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager.Configs;
using CommonHRActionReusables.DateDefinerManager.Exceptions;
using CommonHRActionReusables.DateDefinerManager;
using System.Data.SqlClient;
using CommonDatabaseActionReusables.GeneralUtilities.InputConstraints;

namespace CommonHRActionReusables.DateDefinerManager.Actions
{
    public class CreateDayWithDefinitionAction : AbstractAction<DateDefinerDatabasePathConfig>
    {
        internal CreateDayWithDefinitionAction(DateDefinerDatabasePathConfig config) : base(config)
        {

        }

        //

        /// <summary>
        /// Creates a date with definition in the database indicating that the date with definition is created.
        /// The date with definition is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="InputStringConstraintsViolatedException"></exception>
        /// <returns>The id of the created date with definition</returns>
        public int CreateDateWithDefinition(DayWithDefinition.Builder builder)
        {
            var inputConstraintsChecker = new AntiSQLInjectionInputConstraint();
            inputConstraintsChecker.SatisfiesConstraint(builder.DayTitle);

            //

            int dayDefId = -1;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("INSERT INTO [{0}] ({1}, {2}, {3}, {4}, {5}) VALUES (@DateTimeOfDay, @DayTitle, @DayDesc, @DayType, @RepeatPerYear); SELECT SCOPE_IDENTITY()",
                        databasePathConfig.DateDefinerTableName,
                        databasePathConfig.DateTimeOfDayColumnName,
                        databasePathConfig.DayTitleColumnName, databasePathConfig.DayDescriptionColumnName,
                        databasePathConfig.DayTypeColumnName, databasePathConfig.RepeatPerYearColumnName
                    );

                    command.Parameters.Add(new SqlParameter("DateTimeOfDay", builder.DateTimeOfDay));
                    command.Parameters.Add(new SqlParameter("DayTitle", builder.DayTitle));
                    command.Parameters.Add(new SqlParameter("DayDesc", GetParamOrDbNullIfParamIsNull(builder.DayDescription)));
                    command.Parameters.Add(new SqlParameter("DayType", builder.DayType));
                    command.Parameters.Add(new SqlParameter("RepeatPerYear", builder.RepeatPerYear));

                    dayDefId = int.Parse(command.ExecuteScalar().ToString());
                }
            }

            return dayDefId;
        }


        /// <summary>
        /// Creates a date with definition in the database indicating that the date with definition is created.
        /// The date with definition is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>The id of the created date with definition, or -1 if an exception has occurred</returns>
        public int TryCreateDateWithDefinition(DayWithDefinition.Builder builder)
        {
            try
            {
                return CreateDateWithDefinition(builder);
            }
            catch (Exception)
            {
                return -1;
            }
        }

    }
}
