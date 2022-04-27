using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CommonHRActionReusables_UseCases.Accessors;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.DateDefinerManager;
using CommonDatabaseActionReusables.GeneralUtilities.TypeUtilities;

namespace CommonHRActionReusables_UnitTests.Tests
{
    [TestClass]
    public class TEST_DateDefinerActionTest
    {

        static PortalDateDefinerAccessor dateDefinerAccessor;

        static DayWithDefinition.Builder builder01;
        static DayWithDefinition.Builder builder02;
        static DayWithDefinition.Builder builder03;
        static DayWithDefinition.Builder builderWithInvalidTitle;
        static DayWithDefinition.Builder builder100;
        static DayWithDefinition.Builder builder200;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            dateDefinerAccessor = new PortalDateDefinerAccessor();

            var dateNow = DateTime.Now;

            builder01 = new DayWithDefinition.Builder();
            builder01.DateTimeOfDay = dateNow.AddDays(1);
            builder01.DayTitle = "Valid title 001 here";
            builder01.SetDescriptionUsingString("Description anything goes in here : -- DELETE");
            builder01.DayType = DayTypes.REGULAR;
            builder01.RepeatPerYear = false;

            //

            builder02 = new DayWithDefinition.Builder();
            builder02.DateTimeOfDay = dateNow.AddDays(2);
            builder02.DayTitle = "Valid title 002 here";
            builder02.SetDescriptionUsingString("Description anything goes in here : For 002!!");
            builder02.DayType = DayTypes.HOLIDAY;
            builder02.RepeatPerYear = false;

            //

            builder03 = new DayWithDefinition.Builder();
            builder03.DateTimeOfDay = dateNow.AddDays(3);
            builder03.DayTitle = "Valid title 003 here";
            builder03.SetDescriptionUsingString("Description anything goes in here : 003 times the mi!!");
            builder03.DayType = DayTypes.REGULAR;
            builder03.RepeatPerYear = true;

            //

            builderWithInvalidTitle = new DayWithDefinition.Builder();
            builderWithInvalidTitle.DateTimeOfDay = dateNow.AddDays(10);
            builderWithInvalidTitle.DayTitle = "InValid title 003 here; -- []";
            builderWithInvalidTitle.SetDescriptionUsingString("Description anything goes in here Invalid title however!");
            builderWithInvalidTitle.DayType = DayTypes.REGULAR;
            builderWithInvalidTitle.RepeatPerYear = false;

            //

            builder100 = new DayWithDefinition.Builder();
            builder100.DateTimeOfDay = dateNow.AddYears(1);
            builder100.DayTitle = "Valid title 100 here";
            builder100.SetDescriptionUsingString("Description anything goes in here 100");
            builder100.DayType = DayTypes.REGULAR;
            builder100.RepeatPerYear = true;

            //

            builder200 = new DayWithDefinition.Builder();
            builder200.DateTimeOfDay = dateNow.AddYears(2);
            builder200.DayTitle = "Valid title 200 here";
            builder200.SetDescriptionUsingString("Description anything goes in here 200");
            builder200.DayType = DayTypes.HOLIDAY;
            builder200.RepeatPerYear = true;

        }

        [TestCleanup]
        public void TestMethodCleanup()
        {
            dateDefinerAccessor.DateDefinerManagerHelper.DeleteAllDayWithDefinitions();
        }


        [ClassCleanup]
        public static void ClassCleanup()
        {
            
        }

        //

        #region "Create"

        [TestMethod]
        public void CreatingDayWithDefinition_WithValidTitle_ShouldSucceed()
        {
            var dayId = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);

            Assert.AreNotEqual(-1, dayId);
        }

        [TestMethod]
        public void CreatingDayWithDefinition_WithUnValidTitle_ShouldFail()
        {
            var dayId = dateDefinerAccessor.DateDefinerManagerHelper.TryCreateDateWithDefinition(builderWithInvalidTitle);

            Assert.AreEqual(-1, dayId);
        }

        #endregion


        #region "If exists"

        [TestMethod]
        public void IfDayWithDefinitionExists_WithExistingId_ShouldReturnTrue()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var isExists = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId01);

            Assert.IsTrue(isExists);

        }


        [TestMethod]
        public void IfDayWithDefinitionExists_WithNonExistingId_ShouldReturnFalse()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var isExists = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(-1);

            Assert.IsFalse(isExists);

        }

        #endregion


        #region "Delete All"

        [TestMethod]
        public void DeletingAllDayWithDefinition_With2Originally_ShouldReturnTrue_AndLeaveNone()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var isOriginallyExists01 = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId01);

            var deleteSuccess = dateDefinerAccessor.DateDefinerManagerHelper.DeleteAllDayWithDefinitions();

            var isAfterExists01 = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId01);

            Assert.IsTrue(isOriginallyExists01);
            Assert.IsTrue(deleteSuccess);
            Assert.IsFalse(isAfterExists01);
        }


        [TestMethod]
        public void DeletingAllDayWithDefinition_With0Originally_ShouldReturnTrue_AndLeaveNone()
        {
            var deleteSuccess = dateDefinerAccessor.DateDefinerManagerHelper.DeleteAllDayWithDefinitions();

            Assert.IsTrue(deleteSuccess);
        }

        #endregion


        #region "Delete"

        [TestMethod]
        public void Deleting_ExistingDayWithDefinition_ShouldReturnTrue_AndDeleteOne()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var isOriginallyExists01 = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId01);

            var deleteSuccess01 = dateDefinerAccessor.DateDefinerManagerHelper.DeleteDayWithDefinitionWithId(dayId01);

            var isAfterExists01 = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId01);
            var isAfterExists02 = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId02);


            Assert.IsTrue(isOriginallyExists01);
            Assert.IsTrue(deleteSuccess01);
            Assert.IsFalse(isAfterExists01);
            Assert.IsTrue(isAfterExists02);
        }

        [TestMethod]
        public void Deleting_NonExistingDayWithDefinition_ShouldFail_AndDeleteNone()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var isOriginallyExists01 = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId01);

            var deleteSuccess01 = dateDefinerAccessor.DateDefinerManagerHelper.TryDeleteDayWithDefinitionWithId(-1);

            var isAfterExists01 = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId01);
            var isAfterExists02 = dateDefinerAccessor.DateDefinerManagerHelper.IfDayWithDefinitionIdExsists(dayId02);


            Assert.IsTrue(isOriginallyExists01);
            Assert.IsFalse(deleteSuccess01);
            Assert.IsTrue(isAfterExists01);
            Assert.IsTrue(isAfterExists02);
        }


        #endregion


        #region "Get"

        [TestMethod]
        public void Getting_ExistingDayWithDefinition_ShouldReturnAppropriteObject()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var dayWithDef = dateDefinerAccessor.DateDefinerManagerHelper.GetDayWithDefinitionFromId(dayId01);

            Assert.AreEqual(dayId01, dayWithDef.Id);
            Assert.AreEqual(builder01.DayTitle, dayWithDef.DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), dayWithDef.GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, dayWithDef.DayType);
            Assert.AreEqual(builder01.RepeatPerYear, dayWithDef.RepeatPerYear);
        }

        [TestMethod]
        public void Getting_NonExistingDayWithDefinition_ShouldReturnNull()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var dayWithDef = dateDefinerAccessor.DateDefinerManagerHelper.TryGetDayWithDefinitionFromId(-1);

            Assert.IsNull(dayWithDef);
        }

        #endregion


        #region "Edit"

        [TestMethod]
        public void Editing_ExistingDayWithDefinition_WithValidNewTitle_ShouldReturnTrue()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var successfulEditOf01 = dateDefinerAccessor.DateDefinerManagerHelper.EditDayWithDefinition(dayId01, builder03);
            var dayWithDef01 = dateDefinerAccessor.DateDefinerManagerHelper.GetDayWithDefinitionFromId(dayId01);

            Assert.AreEqual(dayId01, dayWithDef01.Id);
            Assert.AreEqual(builder03.DayTitle, dayWithDef01.DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder03.DayDescription), dayWithDef01.GetDescriptionAsString());
            Assert.AreEqual(builder03.DayType, dayWithDef01.DayType);
            Assert.AreEqual(builder03.RepeatPerYear, dayWithDef01.RepeatPerYear);

            Assert.IsTrue(successfulEditOf01);
        }


        [TestMethod]
        public void Editing_ExistingDayWithDefinition_WithInvalidNewTitle_ShouldFail()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var successfulEditOf01 = dateDefinerAccessor.DateDefinerManagerHelper.TryEditDayWithDefinition(dayId01, builderWithInvalidTitle);
            var dayWithDef01 = dateDefinerAccessor.DateDefinerManagerHelper.GetDayWithDefinitionFromId(dayId01);

            Assert.AreEqual(dayId01, dayWithDef01.Id);
            Assert.AreEqual(builder01.DayTitle, dayWithDef01.DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), dayWithDef01.GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, dayWithDef01.DayType);
            Assert.AreEqual(builder01.RepeatPerYear, dayWithDef01.RepeatPerYear);

            Assert.IsFalse(successfulEditOf01);
        }


        [TestMethod]
        public void Editing_NonExistingDayWithDefinition_ShouldFail()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);

            var successfulEditOf01 = dateDefinerAccessor.DateDefinerManagerHelper.TryEditDayWithDefinition(-1, builder03);
            var dayWithDef01 = dateDefinerAccessor.DateDefinerManagerHelper.GetDayWithDefinitionFromId(dayId01);

            Assert.AreEqual(dayId01, dayWithDef01.Id);
            Assert.AreEqual(builder01.DayTitle, dayWithDef01.DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), dayWithDef01.GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, dayWithDef01.DayType);
            Assert.AreEqual(builder01.RepeatPerYear, dayWithDef01.RepeatPerYear);

            Assert.IsFalse(successfulEditOf01);
        }

        #endregion

        #region "Advanced Get"


        [TestMethod]
        public void AdvancedGetting_FocusDayDefGetParams_DateRangeAllEncompassing_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();

            var dayGetParam = new DayWithDefinitionParameters();
            dayGetParam.DateTimeLowerRange = builder01.DateTimeOfDay;
            dayGetParam.DateTimeUpperRange = builder03.DateTimeOfDay;

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(dayId01, list[0].Id);
            Assert.AreEqual(builder01.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, list[0].DayType);
            Assert.AreEqual(builder01.RepeatPerYear, list[0].RepeatPerYear);

            Assert.AreEqual(dayId02, list[1].Id);
            Assert.AreEqual(builder02.DayTitle, list[1].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder02.DayDescription), list[1].GetDescriptionAsString());
            Assert.AreEqual(builder02.DayType, list[1].DayType);
            Assert.AreEqual(builder02.RepeatPerYear, list[1].RepeatPerYear);

            Assert.AreEqual(dayId03, list[2].Id);
            Assert.AreEqual(builder03.DayTitle, list[2].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder03.DayDescription), list[2].GetDescriptionAsString());
            Assert.AreEqual(builder03.DayType, list[2].DayType);
            Assert.AreEqual(builder03.RepeatPerYear, list[2].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusDayDefGetParams_DateLowerRangeOnly_At2_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();

            var dayGetParam = new DayWithDefinitionParameters();
            dayGetParam.DateTimeLowerRange = builder02.DateTimeOfDay;
            //dayGetParam.DateTimeUpperRange = builder03.DateTimeOfDay;

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(2, list.Count);

            Assert.AreEqual(dayId02, list[0].Id);
            Assert.AreEqual(builder02.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder02.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder02.DayType, list[0].DayType);
            Assert.AreEqual(builder02.RepeatPerYear, list[0].RepeatPerYear);

            Assert.AreEqual(dayId03, list[1].Id);
            Assert.AreEqual(builder03.DayTitle, list[1].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder03.DayDescription), list[1].GetDescriptionAsString());
            Assert.AreEqual(builder03.DayType, list[1].DayType);
            Assert.AreEqual(builder03.RepeatPerYear, list[1].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusDayDefGetParams_DateHigherRangeOnly_At1_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();

            var dayGetParam = new DayWithDefinitionParameters();
            //dayGetParam.DateTimeLowerRange = builder02.DateTimeOfDay;
            dayGetParam.DateTimeUpperRange = builder01.DateTimeOfDay;

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(dayId01, list[0].Id);
            Assert.AreEqual(builder01.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, list[0].DayType);
            Assert.AreEqual(builder01.RepeatPerYear, list[0].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusDayDefGetParams_DateRangeWithNothingInBetween_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();

            var dayGetParam = new DayWithDefinitionParameters();
            dayGetParam.DateTimeLowerRange = builder01.DateTimeOfDay.AddDays(-10);
            dayGetParam.DateTimeUpperRange = builder01.DateTimeOfDay.AddDays(-4);

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(0, list.Count);
        }


        [TestMethod]
        public void AdvancedGetting_FocusDayDefGetParams_DateRangeLimitFlipped_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();

            var dayGetParam = new DayWithDefinitionParameters();
            dayGetParam.DateTimeLowerRange = builder03.DateTimeOfDay;
            dayGetParam.DateTimeUpperRange = builder01.DateTimeOfDay;

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(0, list.Count);
        }


        [TestMethod]
        public void AdvancedGetting_BothDayDefGetParamsAndAdGet_DateRangeAllEncompassing_WithAllAdGetParamFields_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();
            adGetParam.Offset = 1;
            adGetParam.Fetch = 2;
            adGetParam.TextToContain = "title";

            var dayGetParam = new DayWithDefinitionParameters();
            dayGetParam.DateTimeLowerRange = builder01.DateTimeOfDay;
            dayGetParam.DateTimeUpperRange = builder03.DateTimeOfDay;

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(2, list.Count);

            Assert.AreEqual(dayId02, list[0].Id);
            Assert.AreEqual(builder02.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder02.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder02.DayType, list[0].DayType);
            Assert.AreEqual(builder02.RepeatPerYear, list[0].RepeatPerYear);

            Assert.AreEqual(dayId03, list[1].Id);
            Assert.AreEqual(builder03.DayTitle, list[1].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder03.DayDescription), list[1].GetDescriptionAsString());
            Assert.AreEqual(builder03.DayType, list[1].DayType);
            Assert.AreEqual(builder03.RepeatPerYear, list[1].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusAdGet_WithOffset_WithFetch_WithTextToContain_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();
            adGetParam.Offset = 1;
            adGetParam.Fetch = 2;
            adGetParam.TextToContain = "title";

            var dayGetParam = new DayWithDefinitionParameters();
            
            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(2, list.Count);

            Assert.AreEqual(dayId02, list[0].Id);
            Assert.AreEqual(builder02.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder02.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder02.DayType, list[0].DayType);
            Assert.AreEqual(builder02.RepeatPerYear, list[0].RepeatPerYear);

            Assert.AreEqual(dayId03, list[1].Id);
            Assert.AreEqual(builder03.DayTitle, list[1].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder03.DayDescription), list[1].GetDescriptionAsString());
            Assert.AreEqual(builder03.DayType, list[1].DayType);
            Assert.AreEqual(builder03.RepeatPerYear, list[1].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusAdGet_WithNoOffset_WithFetch_WithTextToContain_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();
            //adGetParam.Offset = 1;
            adGetParam.Fetch = 2;
            adGetParam.TextToContain = "title";

            var dayGetParam = new DayWithDefinitionParameters();

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(2, list.Count);

            Assert.AreEqual(dayId01, list[0].Id);
            Assert.AreEqual(builder01.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, list[0].DayType);
            Assert.AreEqual(builder01.RepeatPerYear, list[0].RepeatPerYear);

            Assert.AreEqual(dayId02, list[1].Id);
            Assert.AreEqual(builder02.DayTitle, list[1].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder02.DayDescription), list[1].GetDescriptionAsString());
            Assert.AreEqual(builder02.DayType, list[1].DayType);
            Assert.AreEqual(builder02.RepeatPerYear, list[1].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusAdGet_WithOffset_WithNoFetch_WithTextToContain_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();
            adGetParam.Offset = 2;
            //adGetParam.Fetch = 2;
            adGetParam.TextToContain = "title";

            var dayGetParam = new DayWithDefinitionParameters();

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(dayId03, list[0].Id);
            Assert.AreEqual(builder03.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder03.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder03.DayType, list[0].DayType);
            Assert.AreEqual(builder03.RepeatPerYear, list[0].RepeatPerYear);

        }


        [TestMethod]
        public void AdvancedGetting_FocusAdGet_WithNoOffset_WithNoFetch_WithTextToContain_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();
            adGetParam.TextToContain = "2";

            var dayGetParam = new DayWithDefinitionParameters();

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(dayId02, list[0].Id);
            Assert.AreEqual(builder02.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder02.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder02.DayType, list[0].DayType);
            Assert.AreEqual(builder02.RepeatPerYear, list[0].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusAdGet_WithOffset_WithFetch_WithNoTextToContain_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();
            adGetParam.Offset = 0;
            adGetParam.Fetch = 1;
            
            var dayGetParam = new DayWithDefinitionParameters();

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(dayId01, list[0].Id);
            Assert.AreEqual(builder01.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, list[0].DayType);
            Assert.AreEqual(builder01.RepeatPerYear, list[0].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusAdGet_WithNoOffset_WithFetch_WithNoTextToContain_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();
            //adGetParam.Offset = 1;
            adGetParam.Fetch = 2;
            //adGetParam.TextToContain = "title";

            var dayGetParam = new DayWithDefinitionParameters();

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(2, list.Count);

            Assert.AreEqual(dayId01, list[0].Id);
            Assert.AreEqual(builder01.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, list[0].DayType);
            Assert.AreEqual(builder01.RepeatPerYear, list[0].RepeatPerYear);

            Assert.AreEqual(dayId02, list[1].Id);
            Assert.AreEqual(builder02.DayTitle, list[1].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder02.DayDescription), list[1].GetDescriptionAsString());
            Assert.AreEqual(builder02.DayType, list[1].DayType);
            Assert.AreEqual(builder02.RepeatPerYear, list[1].RepeatPerYear);
        }


        [TestMethod]
        public void AdvancedGetting_FocusAdGet_WithNoOffset_WithNoFetch_WithNoTextToContain_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();

            var dayGetParam = new DayWithDefinitionParameters();

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(dayId01, list[0].Id);
            Assert.AreEqual(builder01.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder01.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder01.DayType, list[0].DayType);
            Assert.AreEqual(builder01.RepeatPerYear, list[0].RepeatPerYear);

            Assert.AreEqual(dayId02, list[1].Id);
            Assert.AreEqual(builder02.DayTitle, list[1].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder02.DayDescription), list[1].GetDescriptionAsString());
            Assert.AreEqual(builder02.DayType, list[1].DayType);
            Assert.AreEqual(builder02.RepeatPerYear, list[1].RepeatPerYear);

            Assert.AreEqual(dayId03, list[2].Id);
            Assert.AreEqual(builder03.DayTitle, list[2].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder03.DayDescription), list[2].GetDescriptionAsString());
            Assert.AreEqual(builder03.DayType, list[2].DayType);
            Assert.AreEqual(builder03.RepeatPerYear, list[2].RepeatPerYear);
        }



        [TestMethod]
        public void AdvancedGetting_FocusDayDefGetParams_DateRangeYearsAbove_ShouldReturnProperOutput()
        {
            var dayId01 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder01);
            var dayId02 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder02);
            var dayId03 = dateDefinerAccessor.DateDefinerManagerHelper.CreateDateWithDefinition(builder03);

            var adGetParam = new AdvancedGetParameters();

            var dayGetParam = new DayWithDefinitionParameters();
            dayGetParam.DateTimeLowerRange = builder01.DateTimeOfDay.AddYears(3);
            dayGetParam.DateTimeUpperRange = builder03.DateTimeOfDay.AddYears(4);

            var list = dateDefinerAccessor.DateDefinerManagerHelper.AdvancedGetDayWithDefinitionAsList(adGetParam, dayGetParam);

            //
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(dayId03, list[0].Id);
            Assert.AreEqual(builder03.DayTitle, list[0].DayTitle);
            Assert.AreEqual(StringUtilities.ConvertByteArrayToString(builder03.DayDescription), list[0].GetDescriptionAsString());
            Assert.AreEqual(builder03.DayType, list[0].DayType);
            Assert.AreEqual(builder03.RepeatPerYear, list[0].RepeatPerYear);
        }

        #endregion

    }
}
