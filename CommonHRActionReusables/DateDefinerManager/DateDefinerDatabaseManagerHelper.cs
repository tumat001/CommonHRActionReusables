using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.DateDefinerManager.Configs;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager.Actions;

namespace CommonHRActionReusables.DateDefinerManager
{
    public class DateDefinerDatabaseManagerHelper
    {

        public DateDefinerDatabasePathConfig PathConfig { set; get; }

        public DateDefinerDatabaseManagerHelper(DateDefinerDatabasePathConfig argPathConfig)
        {
            PathConfig = argPathConfig;
        }

        //

        #region "Advanced Get"

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
        /// If no order by params are supplied to <paramref name="adGetParameter"/>, then the items will be sorted by their date time, descending</returns>
        public IReadOnlyList<DayWithDefinition> AdvancedGetDayWithDefinitionAsList(AdvancedGetParameters adGetParameter, DayWithDefinitionParameters dayWithDefGetParams)
        {
            return new AdvancedGetDayWithDefinitionAction(PathConfig).AdvancedGetDayWithDefinitionAsList(adGetParameter, dayWithDefGetParams);
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
            return new AdvancedGetDayWithDefinitionAction(PathConfig).TryAdvancedGetDayWithDefinitionAsList(adGetParameter, dayWithDefGetParams);
        }

        #endregion

        #region "Create"

        /// <summary>
        /// Creates a date with definition in the database indicating that the date with definition is created.
        /// The date with definition is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>The id of the created date with definition</returns>
        public int CreateDateWithDefinition(DayWithDefinition.Builder builder)
        {
            return new CreateDayWithDefinitionAction(PathConfig).CreateDateWithDefinition(builder);
        }

        /// <summary>
        /// Creates a date with definition in the database indicating that the date with definition is created.
        /// The date with definition is created in the database and table specified in this object's <see cref="DatabasePathConfig"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>The id of the created date with definition, or -1 if an exception has occurred</returns>
        public int TryCreateDateWithDefinition(DayWithDefinition.Builder builder)
        {
            return new CreateDayWithDefinitionAction(PathConfig).TryCreateDateWithDefinition(builder);
        }

        #endregion

        #region "Exists"

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
            return new DayWithDefinitionExistsAction(PathConfig).IfDayWithDefinitionIdExsists(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the <see cref="DayWithDefinition"/> id exists in the given parameters of <see cref="DatabasePathConfig"/></returns>
        public bool TryIfDayWithDefinitionIdExsists(int id)
        {
            return new DayWithDefinitionExistsAction(PathConfig).TryIfDayWithDefinitionIdExsists(id);
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
        /// <returns>A <see cref="DayWithDefinition"/> object containing information about the day with the provided <paramref name="id"/>.</returns>
        public DayWithDefinition GetDayWithDefinitionFromId(int id)
        {
            return new GetDayWithDefinitionAction(PathConfig).GetDayWithDefinitionFromId(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="DayWithDefinition"/> object containing information about the day with the provided <paramref name="id"/>.</returns>
        public DayWithDefinition TryGetDayWithDefinitionFromId(int id)
        {
            return new GetDayWithDefinitionAction(PathConfig).TryGetDayWithDefinitionFromId(id);
        }

        #endregion

        #region "Delete All"

        /// <summary>
        /// Deletes all day with definitions.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <returns>True if the delete operation was successful, even if no day with definition was deleted.</returns>
        public bool DeleteAllDayWithDefinitions()
        {
            return new DeleteAllDayWithDefinitionAction(PathConfig).DeleteAllDayWithDefinitions();
        }

        /// <summary>
        /// Deletes all day with definitions.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <returns>True if the delete operation was successful, even if no day with definition was deleted.</returns>
        public bool TryDeleteAllDayWithDefinitions()
        {
            return new DeleteAllDayWithDefinitionAction(PathConfig).TryDeleteAllDayWithDefinitions();
        }

        #endregion

        #region "Delete"

        /// <summary>
        /// Deletes the day with definition with the given <paramref name="id"/>.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="DayWithDefinitionDoesNotExistException"></exception>
        /// <returns>True if the delete operation was successful. False otherwise.</returns>
        public bool DeleteDayWithDefinitionWithId(int id)
        {
            return new DeleteDayWithDefinitionAction(PathConfig).DeleteDayWithDefinitionWithId(id);
        }

        /// <summary>
        /// Deletes the day with definition with the given <paramref name="id"/>.<br/>
        /// This object's <see cref="DatabasePathConfig"/> determines which database and table is affected.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="DayWithDefinitionDoesNotExistException"></exception>
        /// <returns>True if the delete operation was successful. False otherwise.</returns>
        public bool TryDeleteDayWithDefinitionWithId(int id)
        {
            return new DeleteDayWithDefinitionAction(PathConfig).TryDeleteDayWithDefinitionWithId(id);
        }

        #endregion

        #region "Edit"

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
            return new EditDayWithDefinitionAction(PathConfig).EditDayWithDefinition(id, builder);
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
            return new EditDayWithDefinitionAction(PathConfig).TryEditDayWithDefinition(id, builder);
        }

        #endregion

    }
}
