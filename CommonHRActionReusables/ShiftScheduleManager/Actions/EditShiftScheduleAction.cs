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
    public class EditShiftScheduleAction : AbstractAction<ShiftScheduleDatabasePathConfig>
    {

        internal EditShiftScheduleAction(ShiftScheduleDatabasePathConfig config) : base(config)
        {

        }

        //


        /// <summary>
        /// Edits the <see cref="ShiftSchedule"/> with the provided <paramref name="id"/> using the properties found in <paramref name="builder"/>.<br/>
        /// Setting <paramref name="builder"/> to null makes no edits to the shift schedule.<br/><br/>
        /// Does not edit the employee id field.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="ShiftScheduleDoesNotExistException"></exception>
        /// <exception cref="ShiftScheduleOverlapException"></exception>
        /// <returns>True if the shift schedule was edited successfully, even if <paramref name="builder"/> is set to null.</returns>
        public bool EditShiftSchedule(int id, ShiftSchedule.Builder builder)
        {
            bool idExists = new ShiftScheduleExistsAction(databasePathConfig).IfShiftScheduleIdExsists(id);
            if (!idExists)
            {
                throw new ShiftScheduleDoesNotExistException(id);
            }

            if (builder == null)
            {
                return true;
            }

            //
            var blacklistedIds = new List<int>();
            blacklistedIds.Add(id);

            var overlaps = new IsShiftOverlappingAction(databasePathConfig).IfShiftOverlapsWithExistingShifts(builder, blacklistedIds);
            if (overlaps)
            {
                throw new ShiftScheduleOverlapException();
            }

            //


            var success = false;

            using (SqlConnection sqlConn = databasePathConfig.GetSQLConnection())
            {
                sqlConn.Open();

                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandText = String.Format("UPDATE [{0}] SET [{1}] = @NewDayOfWeek, " +
                        "[{2}] = @NewTimeStart, [{3}] = @NewTimeEnd WHERE [{4}] = @IdVal",
                        databasePathConfig.TableName,

                        databasePathConfig.DayOfWeekColumnName, databasePathConfig.TimeStartColumnName,
                        databasePathConfig.TimeEndColumnName,
                        databasePathConfig.SchedIdColumnName);

                    command.Parameters.Add(new SqlParameter("NewDayOfWeek", (int) builder.DayOfWeek));
                    command.Parameters.Add(new SqlParameter("NewTimeStart", builder.TimeStart));
                    command.Parameters.Add(new SqlParameter("NewTimeEnd", builder.TimeEnd));
                    command.Parameters.Add(new SqlParameter("IdVal", id));

                    success = command.ExecuteNonQuery() > 0;
                }
            }

            return success;

        }


        /// <summary>
        /// Edits the <see cref="ShiftSchedule"/> with the provided <paramref name="id"/> using the properties found in <paramref name="builder"/>.<br/>
        /// Setting <paramref name="builder"/> to null makes no edits to the shift schedule.<br/><br/>
        /// Does not edit the employee id field.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="builder"></param>
        /// <returns>True if the shift schedule was edited successfully, even if <paramref name="builder"/> is set to null.</returns>
        public bool TryEditShiftSchedule(int id, ShiftSchedule.Builder builder)
        {
            try
            {
                return EditShiftSchedule(id, builder);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
