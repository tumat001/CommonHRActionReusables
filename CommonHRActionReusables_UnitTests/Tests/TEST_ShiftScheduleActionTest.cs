using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CommonHRActionReusables_UseCases.Accessors;
using CommonDatabaseActionReusables.AccountManager;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;
using CommonHRActionReusables.ShiftScheduleManager;

namespace CommonHRActionReusables_UnitTests.Tests
{
    [TestClass]
    public class TEST_ShiftScheduleActionTest
    {

        

        private static PortalAccountAccessor portalAccountAccessor;
        private static PortalShiftScheduleAccessor portalShiftScheduleAccessor;

        private const string ACCOUNT_TYPE_EMPLOYEE = "Employee";
        private const string ACCOUNT_TYPE_ADMIN = "Admin";

        private static int employee001Id;
        private static int employee002Id;
        private static int nonExistingEmployeeId;

        private static ShiftSchedule.Builder sBuilder01_01;
        private static ShiftSchedule.Builder sBuilder01_02;
        private static ShiftSchedule.Builder sBuilder01_OverExactOf02;
        private static ShiftSchedule.Builder sBuilder01_OverStartInMiddle_EndInMiddle_Of02;
        private static ShiftSchedule.Builder sBuilder01_OverStartBefore_EndInMiddleOf02;
        private static ShiftSchedule.Builder sBuilder01_OverStartBefore_EndAfterOf02;
        private static ShiftSchedule.Builder sBuilder01_OverStartInMiddle_EndAfterOf02;
        private static ShiftSchedule.Builder sBuilder01_03;

        #region "Init"


        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            portalAccountAccessor = new PortalAccountAccessor();
            portalShiftScheduleAccessor = new PortalShiftScheduleAccessor();
            
            var builder01 = new Account.Builder();
            builder01.AccountType = ACCOUNT_TYPE_EMPLOYEE;
            builder01.Username = "Employee01";
            builder01.Email = "";

            employee001Id = portalAccountAccessor.EmployeeAccountManagerHelper.CreateAccount(builder01, "randPassowrd");

            //

            var builder02 = new Account.Builder();
            builder02.AccountType = ACCOUNT_TYPE_EMPLOYEE;
            builder02.Username = "Employee02";
            builder02.Email = "";

            employee002Id = portalAccountAccessor.EmployeeAccountManagerHelper.CreateAccount(builder02, "randPassowrd");

            //

            nonExistingEmployeeId = -1;

            //

            sBuilder01_01 = new ShiftSchedule.Builder();
            sBuilder01_01.DayOfWeek = DayOfWeek.Tuesday;
            sBuilder01_01.TimeStart = new DateTime(1, 1, 1, 6, 0, 0); //6 am
            sBuilder01_01.TimeEnd = new DateTime(1, 1, 1, 7, 0, 0); //7 am
            sBuilder01_01.EmployeeId = employee001Id;

            sBuilder01_02 = new ShiftSchedule.Builder();
            sBuilder01_02.DayOfWeek = DayOfWeek.Tuesday;
            sBuilder01_02.TimeStart = new DateTime(1, 1, 1, 8, 0, 0); //8 am
            sBuilder01_02.TimeEnd = new DateTime(1, 1, 1, 17, 0, 0); //5 pm
            sBuilder01_02.EmployeeId = employee001Id;

            sBuilder01_OverExactOf02 = new ShiftSchedule.Builder();
            sBuilder01_OverExactOf02.DayOfWeek = DayOfWeek.Tuesday;
            sBuilder01_OverExactOf02.TimeStart = new DateTime(1, 1, 1, 8, 0, 0); //8 am
            sBuilder01_OverExactOf02.TimeEnd = new DateTime(1, 1, 1, 17, 0, 0); //5 pm
            sBuilder01_OverExactOf02.EmployeeId = employee001Id;

            sBuilder01_OverStartInMiddle_EndInMiddle_Of02 = new ShiftSchedule.Builder();
            sBuilder01_OverStartInMiddle_EndInMiddle_Of02.DayOfWeek = DayOfWeek.Tuesday;
            sBuilder01_OverStartInMiddle_EndInMiddle_Of02.TimeStart = new DateTime(1, 1, 1, 10, 0, 0); //10 am
            sBuilder01_OverStartInMiddle_EndInMiddle_Of02.TimeEnd = new DateTime(1, 1, 1, 12, 0, 0); //12 nn
            sBuilder01_OverStartInMiddle_EndInMiddle_Of02.EmployeeId = employee001Id;

            sBuilder01_OverStartBefore_EndInMiddleOf02 = new ShiftSchedule.Builder();
            sBuilder01_OverStartBefore_EndInMiddleOf02.DayOfWeek = DayOfWeek.Tuesday;
            sBuilder01_OverStartBefore_EndInMiddleOf02.TimeStart = new DateTime(1, 1, 1, 6, 0, 0); //6 am
            sBuilder01_OverStartBefore_EndInMiddleOf02.TimeEnd = new DateTime(1, 1, 1, 9, 0, 0); //9 am
            sBuilder01_OverStartBefore_EndInMiddleOf02.EmployeeId = employee001Id;

            sBuilder01_OverStartBefore_EndAfterOf02 = new ShiftSchedule.Builder();
            sBuilder01_OverStartBefore_EndAfterOf02.DayOfWeek = DayOfWeek.Tuesday;
            sBuilder01_OverStartBefore_EndAfterOf02.TimeStart = new DateTime(1, 1, 1, 7, 0, 0); //7 am
            sBuilder01_OverStartBefore_EndAfterOf02.TimeEnd = new DateTime(1, 1, 1, 19, 0, 0); //7 pm
            sBuilder01_OverStartBefore_EndAfterOf02.EmployeeId = employee001Id;

            sBuilder01_OverStartInMiddle_EndAfterOf02 = new ShiftSchedule.Builder();
            sBuilder01_OverStartInMiddle_EndAfterOf02.DayOfWeek = DayOfWeek.Tuesday;
            sBuilder01_OverStartInMiddle_EndAfterOf02.TimeStart = new DateTime(1, 1, 1, 10, 0, 0); //10 am
            sBuilder01_OverStartInMiddle_EndAfterOf02.TimeEnd = new DateTime(1, 1, 1, 19, 0, 0); //7 pm
            sBuilder01_OverStartInMiddle_EndAfterOf02.EmployeeId = employee001Id;

            sBuilder01_03 = new ShiftSchedule.Builder();
            sBuilder01_03.DayOfWeek = DayOfWeek.Wednesday;
            sBuilder01_03.TimeStart = new DateTime(1, 1, 1, 8, 0, 0); //8 am
            sBuilder01_03.TimeEnd = new DateTime(1, 1, 1, 17, 0, 0); //5 pm
            sBuilder01_03.EmployeeId = employee001Id;

        }

        [TestCleanup]
        public void TestMethodCleanup()
        {
            portalShiftScheduleAccessor.ShiftScheduleManagerHelper.DeleteAllShiftSchedules();
        }


        [ClassCleanup]
        public static void ClassCleanup()
        {
            portalAccountAccessor.EmployeeAccountManagerHelper.DeleteAllAccounts();
        }

        #endregion

        //

        #region "Create & IsOverlapping"

        [TestMethod()]
        public void CreatingShiftSchedule_WithNoOverlap_ShouldSucceed()
        {
            var schedId = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);

            Assert.AreNotEqual(-1, schedId);
            Assert.AreNotEqual(-1, schedId02);
        }

        [TestMethod()]
        public void CreatingShiftSchedule_WithOverlap_DETAILS_OverlapExactTime_ShouldFail()
        {
            var schedId = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryCreateShiftSchedule(sBuilder01_OverExactOf02);

            Assert.AreNotEqual(-1, schedId);
            Assert.AreEqual(-1, schedId02);
        }

        [TestMethod()]
        public void CreatingShiftSchedule_WithOverlap_DETAILS_StartsInMiddle_EndsInMiddle_ShouldFail()
        {
            var schedId = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryCreateShiftSchedule(sBuilder01_OverStartInMiddle_EndInMiddle_Of02);

            Assert.AreNotEqual(-1, schedId);
            Assert.AreEqual(-1, schedId02);
        }

        
        [TestMethod()]
        public void CreatingShiftSchedule_WithOverlap_DETAILS_StartsBefore_EndsInMiddle_ShouldFail()
        {
            var schedId = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryCreateShiftSchedule(sBuilder01_OverStartBefore_EndInMiddleOf02);

            Assert.AreNotEqual(-1, schedId);
            Assert.AreEqual(-1, schedId02);
        }


        [TestMethod()]
        public void CreatingShiftSchedule_WithOverlap_DETAILS_StartsBefore_EndsAfter_ShouldFail()
        {
            var schedId = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryCreateShiftSchedule(sBuilder01_OverStartBefore_EndAfterOf02);

            Assert.AreNotEqual(-1, schedId);
            Assert.AreEqual(-1, schedId02);
        }


        [TestMethod()]
        public void CreatingShiftSchedule_WithOverlap_DETAILS_StartsInMiddle_EndsAfter_ShouldFail()
        {
            var schedId = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryCreateShiftSchedule(sBuilder01_OverStartInMiddle_EndAfterOf02);

            Assert.AreNotEqual(-1, schedId);
            Assert.AreEqual(-1, schedId02);
        }

        #endregion

        #region "Is Existing"

        [TestMethod()]
        public void IfShiftSchedExists_WithExistingId_ShouldReturnTrue()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);

            var isExists = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.IfShiftScheduleIdExsists(schedId01);

            Assert.IsTrue(isExists);

        }

        [TestMethod()]
        public void IfShiftSchedExists_WithNonExistingId_ShouldReturnTrue()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);

            var isExists = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.IfShiftScheduleIdExsists(-1);

            Assert.IsFalse(isExists);

        }

        #endregion

        #region "Delete All"

        [TestMethod]
        public void DeletingAll_With2Originally_ShouldReturnTrue_AndLeaveNone()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);

            var isOriginallyExists01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.IfShiftScheduleIdExsists(schedId01);

            var deleteSuccess = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.DeleteAllShiftSchedules();

            var isAfterExists01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.IfShiftScheduleIdExsists(schedId01);

            Assert.IsTrue(isOriginallyExists01);
            Assert.IsTrue(deleteSuccess);
            Assert.IsFalse(isAfterExists01);
        }

        [TestMethod]
        public void DeletingAll_With0Originally_ShouldReturnTrue_AndLeaveNone()
        {
            
            var deleteSuccess = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.DeleteAllShiftSchedules();

            Assert.IsTrue(deleteSuccess);
        }

        #endregion

        #region "Delete"

        [TestMethod]
        public void Deleting_ExistingSched_ShouldReturnTrue_AndDeleteOne()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);

            var isOriginallyExists01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.IfShiftScheduleIdExsists(schedId01);

            var deleteSuccess01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.DeleteShiftScheduleWithId(schedId01);

            var isAfterExists01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.IfShiftScheduleIdExsists(schedId01);

            Assert.IsTrue(isOriginallyExists01);
            Assert.IsTrue(deleteSuccess01);
            Assert.IsFalse(isAfterExists01);
        }

        [TestMethod]
        public void Deleting_NonExistingSched_ShouldFail_AndDeleteNone()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);

            var isOriginallyExists01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.IfShiftScheduleIdExsists(schedId01);

            var deleteSuccess01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryDeleteShiftScheduleWithId(-1);

            var isAfterExists01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.IfShiftScheduleIdExsists(schedId01);

            Assert.IsTrue(isOriginallyExists01);
            Assert.IsFalse(deleteSuccess01);
            Assert.IsTrue(isAfterExists01);
        }

        #endregion

        #region "Get"

        [TestMethod]
        public void Getting_ExistingSched_ShouldReturnAppropriteObject()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);

            var shiftSched01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.GetShiftScheduleFromId(schedId01);

            Assert.AreEqual(schedId01, shiftSched01.SchedId);
            Assert.AreEqual(sBuilder01_01.EmployeeId, shiftSched01.EmployeeId);
            Assert.AreEqual(sBuilder01_01.DayOfWeek, shiftSched01.DayOfWeek);
            Assert.AreEqual(0, DateTime.Compare(sBuilder01_01.TimeStart, shiftSched01.TimeStart));
            Assert.AreEqual(0, DateTime.Compare(sBuilder01_01.TimeEnd, shiftSched01.TimeEnd));
        }

        [TestMethod]
        public void Getting_NonExistingSched_ShouldReturnNull()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);

            var shiftSched01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryGetShiftScheduleFromId(-1);

            Assert.IsNull(shiftSched01);
        }

        #endregion

        #region "Edit"

        [TestMethod]
        public void Editing_ExistingShiftSched_ShouldReturnTrue()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);

            var successfulEditOf01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.EditShiftSchedule(schedId01, sBuilder01_02);
            var shiftSched01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.GetShiftScheduleFromId(schedId01);

            Assert.AreEqual(schedId01, shiftSched01.SchedId);
            Assert.AreEqual(sBuilder01_02.EmployeeId, shiftSched01.EmployeeId);
            Assert.AreEqual(sBuilder01_02.DayOfWeek, shiftSched01.DayOfWeek);
            Assert.AreEqual(0, DateTime.Compare(sBuilder01_02.TimeStart, shiftSched01.TimeStart));
            Assert.AreEqual(0, DateTime.Compare(sBuilder01_02.TimeEnd, shiftSched01.TimeEnd));

            Assert.IsTrue(successfulEditOf01);
        }

        [TestMethod]
        public void Editing_NonExistingShiftSched_ShouldFail()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);

            var successfulEdit = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryEditShiftSchedule(-1, sBuilder01_01);
            var shiftSched01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.GetShiftScheduleFromId(schedId01);

            Assert.AreEqual(schedId01, shiftSched01.SchedId);
            Assert.AreEqual(sBuilder01_01.EmployeeId, shiftSched01.EmployeeId);
            Assert.AreEqual(sBuilder01_01.DayOfWeek, shiftSched01.DayOfWeek);
            Assert.AreEqual(0, DateTime.Compare(sBuilder01_01.TimeStart, shiftSched01.TimeStart));
            Assert.AreEqual(0, DateTime.Compare(sBuilder01_01.TimeEnd, shiftSched01.TimeEnd));

            Assert.IsFalse(successfulEdit);
        }

        [TestMethod]
        public void Editing_ExistingShiftSched_WithNoChange_ShouldReturnTrue()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);

            var successfulEdit = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.TryEditShiftSchedule(schedId01, sBuilder01_01);
            var shiftSched01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.GetShiftScheduleFromId(schedId01);

            Assert.AreEqual(schedId01, shiftSched01.SchedId);
            Assert.AreEqual(sBuilder01_01.EmployeeId, shiftSched01.EmployeeId);
            Assert.AreEqual(sBuilder01_01.DayOfWeek, shiftSched01.DayOfWeek);
            Assert.AreEqual(0, DateTime.Compare(sBuilder01_01.TimeStart, shiftSched01.TimeStart));
            Assert.AreEqual(0, DateTime.Compare(sBuilder01_01.TimeEnd, shiftSched01.TimeEnd));

            Assert.IsTrue(successfulEdit);
        }

        #endregion

        #region "AdvancedGet"


        [TestMethod]
        public void AdvancedGetting_WithOffset_WithFetch_ShouldReturnProperOutput()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId03 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_03);


            var adGetParam = new AdvancedGetParameters();
            adGetParam.Offset = 1;
            adGetParam.Fetch = 2;

            var list = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.AdvancedGetShiftScheduleAsList(employee001Id, adGetParam);

            //
            Assert.AreEqual(2, list.Count);

            var sched01InList = list[0];
            Assert.AreEqual(schedId02, sched01InList.SchedId);
            Assert.AreEqual(sBuilder01_02.EmployeeId, sched01InList.EmployeeId);
            Assert.AreEqual(sBuilder01_02.DayOfWeek, sched01InList.DayOfWeek);
            Assert.AreEqual(sBuilder01_02.TimeStart, sched01InList.TimeStart);
            Assert.AreEqual(sBuilder01_02.TimeEnd, sched01InList.TimeEnd);

            var sched02InList = list[1];
            Assert.AreEqual(schedId03, sched02InList.SchedId);
            Assert.AreEqual(sBuilder01_03.EmployeeId, sched02InList.EmployeeId);
            Assert.AreEqual(sBuilder01_03.DayOfWeek, sched02InList.DayOfWeek);
            Assert.AreEqual(sBuilder01_03.TimeStart, sched02InList.TimeStart);
            Assert.AreEqual(sBuilder01_03.TimeEnd, sched02InList.TimeEnd);

        }



        [TestMethod]
        public void AdvancedGetting_WithNoOffset_WithFetch_ShouldReturnProperOutput()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId03 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_03);


            var adGetParam = new AdvancedGetParameters();
            //adGetParam.Offset = 1;
            adGetParam.Fetch = 2;

            var list = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.AdvancedGetShiftScheduleAsList(employee001Id, adGetParam);

            //
            Assert.AreEqual(2, list.Count);

            var sched01InList = list[0];
            Assert.AreEqual(schedId01, sched01InList.SchedId);
            Assert.AreEqual(sBuilder01_01.EmployeeId, sched01InList.EmployeeId);
            Assert.AreEqual(sBuilder01_01.DayOfWeek, sched01InList.DayOfWeek);
            Assert.AreEqual(sBuilder01_01.TimeStart, sched01InList.TimeStart);
            Assert.AreEqual(sBuilder01_01.TimeEnd, sched01InList.TimeEnd);

            var sched02InList = list[1];
            Assert.AreEqual(schedId02, sched02InList.SchedId);
            Assert.AreEqual(sBuilder01_02.EmployeeId, sched02InList.EmployeeId);
            Assert.AreEqual(sBuilder01_02.DayOfWeek, sched02InList.DayOfWeek);
            Assert.AreEqual(sBuilder01_02.TimeStart, sched02InList.TimeStart);
            Assert.AreEqual(sBuilder01_02.TimeEnd, sched02InList.TimeEnd);

        }


        [TestMethod]
        public void AdvancedGetting_WithOffset_WithNoFetch_ShouldReturnProperOutput()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId03 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_03);


            var adGetParam = new AdvancedGetParameters();
            adGetParam.Offset = 2;
            //adGetParam.Fetch = 2;

            var list = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.AdvancedGetShiftScheduleAsList(employee001Id, adGetParam);

            //
            Assert.AreEqual(1, list.Count);

            var sched01InList = list[0];
            Assert.AreEqual(schedId03, sched01InList.SchedId);
            Assert.AreEqual(sBuilder01_03.EmployeeId, sched01InList.EmployeeId);
            Assert.AreEqual(sBuilder01_03.DayOfWeek, sched01InList.DayOfWeek);
            Assert.AreEqual(sBuilder01_03.TimeStart, sched01InList.TimeStart);
            Assert.AreEqual(sBuilder01_03.TimeEnd, sched01InList.TimeEnd);
            
        }



        [TestMethod]
        public void AdvancedGetting_WithNoOffset_WithNoFetch_ShouldReturnProperOutput()
        {
            var schedId01 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_01);
            var schedId02 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_02);
            var schedId03 = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.CreateShiftSchedule(sBuilder01_03);


            var adGetParam = new AdvancedGetParameters();
            //adGetParam.Offset = 2;
            //adGetParam.Fetch = 2;

            var list = portalShiftScheduleAccessor.ShiftScheduleManagerHelper.AdvancedGetShiftScheduleAsList(employee001Id, adGetParam);

            //
            Assert.AreEqual(3, list.Count);

            var sched01InList = list[0];
            Assert.AreEqual(schedId01, sched01InList.SchedId);
            Assert.AreEqual(sBuilder01_01.EmployeeId, sched01InList.EmployeeId);
            Assert.AreEqual(sBuilder01_01.DayOfWeek, sched01InList.DayOfWeek);
            Assert.AreEqual(sBuilder01_01.TimeStart, sched01InList.TimeStart);
            Assert.AreEqual(sBuilder01_01.TimeEnd, sched01InList.TimeEnd);

            var sched02InList = list[1];
            Assert.AreEqual(schedId02, sched02InList.SchedId);
            Assert.AreEqual(sBuilder01_02.EmployeeId, sched02InList.EmployeeId);
            Assert.AreEqual(sBuilder01_02.DayOfWeek, sched02InList.DayOfWeek);
            Assert.AreEqual(sBuilder01_02.TimeStart, sched02InList.TimeStart);
            Assert.AreEqual(sBuilder01_02.TimeEnd, sched02InList.TimeEnd);

            var sched03InList = list[2];
            Assert.AreEqual(schedId03, sched03InList.SchedId);
            Assert.AreEqual(sBuilder01_03.EmployeeId, sched03InList.EmployeeId);
            Assert.AreEqual(sBuilder01_03.DayOfWeek, sched03InList.DayOfWeek);
            Assert.AreEqual(sBuilder01_03.TimeStart, sched03InList.TimeStart);
            Assert.AreEqual(sBuilder01_03.TimeEnd, sched03InList.TimeEnd);

        }

        #endregion

    }
}
