using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.AttendanceHistoryManager.Configs;
using CommonHRActionReusables.AttendanceHistoryManager;
using System.Data.SqlClient;

namespace CommonHRActionReusables.AttendanceHistoryManager.Actions
{
    public class AdvancedGetAttendanceHistoryAction : AbstractAction<AttendanceHistoryDatabasePathConfig>
    {

        internal AdvancedGetAttendanceHistoryAction(AttendanceHistoryDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="adGetParameter"></param>
        /// <param name="attendParam"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns>A list of <see cref="AttendanceHistory"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.
        /// <br/>
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, ascending</returns>
        public IReadOnlyList<AttendanceHistory> AdvancedGetAttendanceHistoryAsList(int empId, AdvancedGetParameters adGetParameter, AdvancedGetAttendanceHistoryParameters attendParam)
        {

            var list = new List<AttendanceHistory>();

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    
                    command.CommandText = string.Format("SELECT [{0}], [{1}], [{2}] FROM [{3}] WHERE [{4}] = @IdVal " +
                        "{5} {6} {7} " +
                        "{8} {9} {10}",

                        databasePathConfig.DateTimeOfStartColName, databasePathConfig.TotalMinsColName,
                        databasePathConfig.MinutesPresentColName,

                        databasePathConfig.TableName,

                        GetConjunctionBeforeTextLikeFind(adGetParameter, attendParam),
                        attendParam.GetDateRangeAsQueryString(databasePathConfig.DateTimeOfStartColName),
                        GetConjuctionForWhere(adGetParameter, attendParam),

                        databasePathConfig.EmployeeIdColName,
                        adGetParameter.GetSQLStatementFromOrderBy(databasePathConfig.DateTimeOfStartColName, OrderType.ASCENDING),
                        adGetParameter.GetSQLStatementFromOffset(),
                        adGetParameter.GetSQLStatementFromFetch()
                        );

                    command.Parameters.Add(new SqlParameter("IdVal", empId));



                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var builder = new AttendanceHistory.Builder();

                            var dateTimeStart = reader.GetSqlDateTime(0).Value;
                            var totalMins = reader.GetSqlInt32(1).Value;
                            var minsPresent = reader.GetSqlInt32(2).Value;

                            builder.EmployeeId = empId;
                            builder.DateTimeOfStart = dateTimeStart;
                            builder.TotalMinutes = totalMins;
                            builder.MinutesPresent = minsPresent;

                            var attendanceHistory = builder.build();
                            list.Add(attendanceHistory);
                        }

                    }
                }
            }

            return list;
        }


        private String GetConjunctionBeforeTextLikeFind(AdvancedGetParameters param, AdvancedGetAttendanceHistoryParameters attendParam)
        {
            if (attendParam.DateTimeLowerRange.HasValue || attendParam.DateTimeUpperRange.HasValue)
            {
                return "WHERE";
            }
            else
            {
                return "";
            }
        }

        private String GetConjuctionForWhere(AdvancedGetParameters adGetParam, AdvancedGetAttendanceHistoryParameters attendParam)
        {
            if ((!attendParam.DateTimeLowerRange.HasValue && !attendParam.DateTimeUpperRange.HasValue) || string.IsNullOrEmpty(adGetParam.TextToContain))
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
        /// <param name="empId"></param>
        /// <param name="adGetParameter"></param>
        /// <param name="attendParam"></param>
        /// <returns>A list of <see cref="AttendanceHistory"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.
        /// <br/>
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, ascending</returns>
        public IReadOnlyList<AttendanceHistory> TryAdvancedGetAttendanceHistoryAsList(int empId, AdvancedGetParameters adGetParameter, AdvancedGetAttendanceHistoryParameters attendParam)
        {
            try
            {
                return AdvancedGetAttendanceHistoryAsList(empId, adGetParameter, attendParam);
            }
            catch (Exception)
            {
                return new List<AttendanceHistory>();
            }
        }

    }
}
