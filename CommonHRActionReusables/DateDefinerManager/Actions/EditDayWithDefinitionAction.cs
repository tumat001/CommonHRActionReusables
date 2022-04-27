using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager.Configs;
using System.Data.SqlClient;
using CommonDatabaseActionReusables.GeneralUtilities.TypeUtilities;
using CommonHRActionReusables.DateDefinerManager.Exceptions;
using CommonDatabaseActionReusables.GeneralUtilities.InputConstraints;


namespace CommonHRActionReusables.DateDefinerManager.Actions
{
    public class EditDayWithDefinitionAction : AbstractAction<DateDefinerDatabasePathConfig>
    {

        internal EditDayWithDefinitionAction(DateDefinerDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Edits the day with definition with the provided <paramref name="id"/> using the properties found in <paramref name="builder"/>.<br/>
        /// Setting <paramref name="builder"/> to null makes no edits to the day with definition.<br/><br/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="DayWithDefinitionDoesNotExistException"></exception>
        /// <exception cref="InputStringConstraintsViolatedException"></exception>
        /// <returns>True if the day with definition was edited successfully, even if <paramref name="builder"/> is set to null.</returns>
        public bool EditDayWithDefinition(int id, DayWithDefinition.Builder builder)
        {
            bool idExists = new DayWithDefinitionExistsAction(databasePathConfig).IfDayWithDefinitionIdExsists(id);
            if (!idExists)
            {
                throw new DayWithDefinitionDoesNotExistException(id);
            }

            if (builder == null)
            {
                return true;
            }


            var inputConstraintsChecker = new AntiSQLInjectionInputConstraint();
            inputConstraintsChecker.SatisfiesConstraint(builder.DayTitle);

            //


            var success = false;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = String.Format("UPDATE [{0}] SET [{1}] = @NewDateVal, [{2}] = @NewTitleVal, [{3}] = @NewDescVal, [{4}] = @NewType, [{5}] = @NewIsRepeat WHERE [{6}] = @IdVal",
                        databasePathConfig.DateDefinerTableName,

                        databasePathConfig.DateTimeOfDayColumnName, databasePathConfig.DayTitleColumnName, 
                        databasePathConfig.DayDescriptionColumnName, databasePathConfig.DayTypeColumnName, 
                        databasePathConfig.RepeatPerYearColumnName,
                        databasePathConfig.IdColumnName);

                    command.Parameters.Add(new SqlParameter("NewDateVal", builder.DateTimeOfDay));
                    command.Parameters.Add(new SqlParameter("NewTitleVal", builder.DayTitle));
                    command.Parameters.Add(new SqlParameter("NewDescVal", GetParamOrDbNullIfParamIsNull(builder.DayDescription)));
                    command.Parameters.Add(new SqlParameter("NewType", builder.DayType));
                    command.Parameters.Add(new SqlParameter("NewIsRepeat", builder.RepeatPerYear));
                    command.Parameters.Add(new SqlParameter("IdVal", id));

                    success = command.ExecuteNonQuery() > 0;
                }
            }

            return success;

        }


        /// <summary>
        /// Edits the day with definition with the provided <paramref name="id"/> using the properties found in <paramref name="builder"/>.<br/>
        /// Setting <paramref name="builder"/> to null makes no edits to the day with definition.<br/><br/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="builder"></param>
        /// <returns>True if the day with definition was edited successfully, even if <paramref name="builder"/> is set to null.</returns>
        public bool TryEditDayWithDefinition(int id, DayWithDefinition.Builder builder)
        {
            try
            {
                return EditDayWithDefinition(id, builder);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
