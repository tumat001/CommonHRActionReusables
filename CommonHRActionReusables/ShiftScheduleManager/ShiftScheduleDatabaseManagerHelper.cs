using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.ShiftScheduleManager.Configs;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.ShiftScheduleManager.Actions;

namespace CommonHRActionReusables.ShiftScheduleManager
{
    public class ShiftScheduleDatabaseManagerHelper
    {

        public ShiftScheduleDatabasePathConfig PathConfig { set; get; }

        public ShiftScheduleDatabaseManagerHelper(ShiftScheduleDatabasePathConfig argPathConfig)
        {
            PathConfig = argPathConfig;
        }

        //

        #region "Create"

        /// <summary>
        /// Creates a <see cref="ShiftSchedule"/> in the database.
        /// The shift schedule is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="ShiftScheduleOverlapException"
        /// <returns>The id of the created shift schedule</returns>
        public int CreateShiftSchedule(ShiftSchedule.Builder builder)
        {
            return new CreateShiftScheduleAction(PathConfig).CreateShiftSchedule(builder);
        }

        /// <summary>
        /// Creates a <see cref="ShiftSchedule"/> in the database.
        /// The shift schedule is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="ShiftScheduleOverlapException"
        /// <returns>The id of the created shift schedule, or -1 if an exception has occurred.</returns>
        public int TryCreateShiftSchedule(ShiftSchedule.Builder builder)
        {
            return new CreateShiftScheduleAction(PathConfig).TryCreateShiftSchedule(builder);
        }

        #endregion

        #region "Delete all"

        /// <summary>
        /// Deletes all shift schedules.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the delete operation was successful, even if nothing was deleted.</returns>
        public bool DeleteAllShiftSchedules()
        {
            return new DeleteAllShiftScheduleAction(PathConfig).DeleteAllShiftSchedules();
        }

        /// <summary>
        /// Deletes all shift schedules.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <returns>True if the delete operation was successful, even if nothing was deleted.</returns>public bool TryDeleteAllDayWithDefinitions()
        public bool TryDeleteAllShiftSchedules()
        {
            return new DeleteAllShiftScheduleAction(PathConfig).TryDeleteAllShiftSchedules();
        }

        #endregion

        #region "IsShift Overlapping"


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blacklistedIds">The ids to ignore when looking for overlaps. Null stands for no ids are to be blacklisted</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the <see cref="ShiftSchedule"/>'s time frame overlaps with another shift in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool IfShiftOverlapsWithExistingShifts(ShiftSchedule.Builder shiftSchedule, IList<int> blacklistedIds = null)
        {
            return new IsShiftOverlappingAction(PathConfig).IfShiftOverlapsWithExistingShifts(shiftSchedule, blacklistedIds);
        }

        #endregion

        #region "Sched exists"

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
            return new ShiftScheduleExistsAction(PathConfig).IfShiftScheduleIdExsists(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the <see cref="ShiftSchedule"/> id exists in the given parameters of <see cref="DatabasePathConfig"/>. Returns false if an exception has occurred.</returns>
        public bool TryIfShiftScheduleIdExsists(int id)
        {
            return new ShiftScheduleExistsAction(PathConfig).TryIfShiftScheduleIdExsists(id);
        }

        #endregion

        #region "Delete"

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
            return new DeleteShiftScheduleAction(PathConfig).DeleteShiftScheduleWithId(id);
        }

        /// <summary>
        /// Deletes the shift schedule with the given <paramref name="id"/>.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the delete operation was successful. False otherwise.</returns>
        public bool TryDeleteShiftScheduleWithId(int id)
        {
            return new DeleteShiftScheduleAction(PathConfig).TryDeleteShiftScheduleWithId(id);
        }

        #endregion

        #region "Get"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="DayWithDefinitionDoesNotExistException"></exception>
        /// <returns>A <see cref="ShiftSchedule"/> object containing information about the shift schedule with the provided <paramref name="id"/>.</returns>
        public ShiftSchedule GetShiftScheduleFromId(int id)
        {
            return new GetShiftScheduleAction(PathConfig).GetShiftScheduleFromId(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="ShiftSchedule"/> object containing information about the shift schedule with the provided <paramref name="id"/>.</returns>
        public ShiftSchedule TryGetShiftScheduleFromId(int id)
        {
            return new GetShiftScheduleAction(PathConfig).TryGetShiftScheduleFromId(id);
        }

        #endregion

        #region "Edit"

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
            return new EditShiftScheduleAction(PathConfig).EditShiftSchedule(id, builder);
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
            return new EditShiftScheduleAction(PathConfig).TryEditShiftSchedule(id, builder);
        }

        #endregion

        #region "AdvancedGet"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adGetParameter"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns>A list of <see cref="ShiftSchedule"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.
        /// <br/>
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their day of week and time, ascending</returns>
        public IReadOnlyList<ShiftSchedule> AdvancedGetShiftScheduleAsList(int empId, AdvancedGetParameters adGetParameter)
        {
            return new AdvancedGetShiftScheduleAction(PathConfig).AdvancedGetShiftScheduleAsList(empId, adGetParameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adGetParameter"></param>
        /// <returns>A list of <see cref="ShiftSchedule"/> found in the database given in this object's <see cref="DatabasePathConfig"/>, taking into
        /// account the given <paramref name="adGetParameter"/>.
        /// <br/>
        /// <br/>
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their day of week and time, ascending</returns>
        public IReadOnlyList<ShiftSchedule> TryAdvancedGetShiftScheduleAsList(int empId, AdvancedGetParameters adGetParameter)
        {
            return new AdvancedGetShiftScheduleAction(PathConfig).TryAdvancedGetShiftScheduleAsList(empId, adGetParameter);
        }

        #endregion



    }
}
