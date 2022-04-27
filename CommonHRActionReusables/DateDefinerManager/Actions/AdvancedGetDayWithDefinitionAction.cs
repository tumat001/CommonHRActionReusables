using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager.Configs;
using System.Data.SqlClient;
using CommonDatabaseActionReusables.GeneralUtilities.TypeUtilities;

namespace CommonHRActionReusables.DateDefinerManager.Actions
{
    public class AdvancedGetDayWithDefinitionAction : AbstractAction<DateDefinerDatabasePathConfig>
    {
        internal AdvancedGetDayWithDefinitionAction(DateDefinerDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="adGetParameter"></param>
        /// <param name="dayWithDefGetParams"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns>A list of <see cref="DayWithDefinition"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>. This finds <see cref="DayWithDefinition"/> with the same day, while also considering <see cref="DayWithDefinition.RepeatPerYear"/>.
        /// <br/><br/>
        /// <paramref name="dayWithDefGetParams"/> allows more precision on which day definitions to get.
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, ascending</returns>
        public IReadOnlyList<DayWithDefinition> AdvancedGetDayWithDefinitionAsList(AdvancedGetParameters adGetParameter, DayWithDefinitionParameters dayWithDefGetParams)
        {

            var list = new List<DayWithDefinition>();

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT [{0}], [{1}], [{2}], [{3}], [{4}], [{5}] FROM [{6}] {7} {8} {9} {10} {11} {12} {13}",
                        databasePathConfig.IdColumnName, databasePathConfig.DateTimeOfDayColumnName,
                        databasePathConfig.DayTitleColumnName, databasePathConfig.DayDescriptionColumnName,
                        databasePathConfig.DayTypeColumnName, databasePathConfig.RepeatPerYearColumnName,

                        databasePathConfig.DateDefinerTableName,

                        GetConjunctionBeforeTextLikeFind(adGetParameter, dayWithDefGetParams),
                        adGetParameter.GetSQLStatementFromTextToContain(databasePathConfig.DayTitleColumnName),
                        GetConjuctionForWhere(adGetParameter, dayWithDefGetParams),
                        dayWithDefGetParams.GetDateRangeAsQueryString(databasePathConfig.DateTimeOfDayColumnName, databasePathConfig.RepeatPerYearColumnName),
                        adGetParameter.GetSQLStatementFromOrderBy(databasePathConfig.DateTimeOfDayColumnName, OrderType.ASCENDING),
                        adGetParameter.GetSQLStatementFromOffset(),
                        adGetParameter.GetSQLStatementFromFetch()
                        );

                    Console.WriteLine(command.CommandText);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetSqlInt32(0).Value;
                            var date = reader.GetSqlDateTime(1).Value;
                            var dayTitle = reader.GetSqlString(2).Value;
                            var dayDescription = StringUtilities.ConvertSqlStringToByteArray(reader.GetSqlString(3));
                            var dayType = DayType.GetDayTypeFromNumber(reader.GetSqlInt32(4).Value);
                            var repeatPerYear = reader.GetSqlBoolean(5).Value;

                            var dayWithDef = new DayWithDefinition.Builder();
                            dayWithDef.DateTimeOfDay = date;
                            dayWithDef.DayTitle = dayTitle;
                            dayWithDef.DayDescription = dayDescription;
                            dayWithDef.DayType = dayType;
                            dayWithDef.RepeatPerYear = repeatPerYear;

                            list.Add(dayWithDef.build(id));
                        }

                    }
                }
            }

            return list;
        }


        private String GetConjunctionBeforeTextLikeFind(AdvancedGetParameters param, DayWithDefinitionParameters dayParam)
        {
            if (string.IsNullOrEmpty(param.TextToContain) && (dayParam.DateTimeLowerRange.HasValue || dayParam.DateTimeUpperRange.HasValue))
            {
                return "WHERE";
            }
            else
            {
                return "";
            }
        }

        private String GetConjuctionForWhere(AdvancedGetParameters adGetParam, DayWithDefinitionParameters param)
        {
            if ((!param.DateTimeLowerRange.HasValue && !param.DateTimeUpperRange.HasValue) || string.IsNullOrEmpty(adGetParam.TextToContain))
            {
                return "";
            }
            else
            {
                return "AND";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="adGetParameter"></param>
        /// <param name="dayWithDefGetParams"></param>
        /// <returns>A list of <see cref="DayWithDefinition"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>. This finds <see cref="DayWithDefinition"/> with the same day, while also considering <see cref="DayWithDefinition.RepeatPerYear"/>.
        /// <br/><br/>
        /// <paramref name="dayWithDefGetParams"/> allows more precision on which day definitions to get.
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, descending</returns>
        public IReadOnlyList<DayWithDefinition> TryAdvancedGetDayWithDefinitionAsList(AdvancedGetParameters adGetParameter, DayWithDefinitionParameters dayWithDefGetParams)
        { 
            try
            {
                return AdvancedGetDayWithDefinitionAsList(adGetParameter, dayWithDefGetParams);
            }
            catch (Exception)
            {
                return new List<DayWithDefinition>();
            }
        }

    }
}
