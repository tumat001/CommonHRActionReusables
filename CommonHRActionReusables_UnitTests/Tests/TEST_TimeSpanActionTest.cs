using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CommonHRActionReusables_UseCases.Accessors;
using CommonDatabaseActionReusables.AccountManager;
using CommonDatabaseActionReusables.GeneralUtilities.DatabaseActions;

namespace CommonHRActionReusables_UnitTests.Tests
{
    [TestClass]
    public class TEST_TimeSpanActionTest
    {

        private static PortalAccountAccessor portalAccountAccessor;

        private const string ACCOUNT_TYPE_EMPLOYEE = "Employee";
        private const string ACCOUNT_TYPE_ADMIN = "Admin";

        private static int employee001Id;
        private static int employee002Id;
        private static int nonExistingEmployeeId;


        private static PortalTimeSpanAccessor portalTimeSpanAccessor;


        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            portalAccountAccessor = new PortalAccountAccessor();
            portalTimeSpanAccessor = new PortalTimeSpanAccessor();

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
        }

        [TestCleanup]
        public void TestMethodCleanup()
        {
            portalTimeSpanAccessor.TimeInSpanManagerHelper.DeleteAllTimeSpans();
        }


        [ClassCleanup]
        public static void ClassCleanup()
        {
            portalAccountAccessor.EmployeeAccountManagerHelper.DeleteAllAccounts();
        }

        //


        #region "Start Time In"

        [TestMethod]
        public void StartTimeIn_WithExistingEmployee_WithNoOverlappingStart_ShouldSucceed()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);
        }

        [TestMethod]
        public void StartTimeIn_WithNonExistingEmployee_WithNoOverlappingStart_ShouldFail()
        {
            var dateTimeNow = DateTime.Now;

            try
            {
                portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(nonExistingEmployeeId, dateTimeNow);
                Assert.Fail("Method has not thrown an exception! Employee Id does not exist.");
            }
            catch (Exception)
            {

            }
        }


        [TestMethod]
        public void StartTimeIn_WithExistingEmployee_WithOverlappingStart_ShouldFail()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

            try
            {
                portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);
                Assert.Fail("Method has not thrown an exception! Time span start already exists. End should be next.");
            }
            catch (Exception)
            {

            }
        }


        [TestMethod]
        public void StartTimeIn_WithExistingEmployee_WithNoOverlappingStart_WithPreviousEnd_ShouldFail()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTimeNow);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

        }

        #endregion

        //

        #region "End Time In"

        [TestMethod]
        public void EndTimeIn_WithExistingEmployee_WithPreviousStart_WithNoOverlappingEnd_ShouldSucceed()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTimeNow);
        }

        [TestMethod]
        public void EndTimeIn_WithNonExistingEmployee_WithPreviousStart_WithNoOverlappingEnd_ShouldSucceed()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

            try
            {
                portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(nonExistingEmployeeId, dateTimeNow);
                Assert.Fail("Method has not thrown an exception! Employee Id does not exist.");
            }
            catch (Exception)
            {

            }
        }


        [TestMethod]
        public void EndTimeIn_WithExistingEmployee_WithPreviousStart_WithOverlappingEnd_ShouldSucceed()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTimeNow);

            try
            {
                portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTimeNow);
                Assert.Fail("Method has not thrown an exception! Time span end already exists. Start should be next.");
            }
            catch (Exception)
            {

            }
        }


        #endregion

        //

        #region "Delete All time spans"

        [TestMethod]
        public void DeletingAllTimeSpans_With0TimeSpans_ShouldReturnTrue()
        {
            var success = portalTimeSpanAccessor.TimeInSpanManagerHelper.DeleteAllTimeSpans();

            Assert.IsTrue(success);
        }


        [TestMethod]
        public void DeletingAllTimeSpans_WithXTimeSpans_ShouldReturnTrue()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTimeNow);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

            var success = portalTimeSpanAccessor.TimeInSpanManagerHelper.DeleteAllTimeSpans();

            Assert.IsTrue(success);
        }

        #endregion

        //

        #region "Is Time Start or End"


        [TestMethod]
        public void IsTimeStarted_ForExistingEmployee_WithTimeSpanStarted_ReturnsTrue()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTimeNow);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTimeNow);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee002Id, dateTimeNow);

            //

            var timeStarted = portalTimeSpanAccessor.TimeInSpanManagerHelper.IfTimeIsStartedForEmployee(employee001Id);

            Assert.IsTrue(timeStarted);
        }


        [TestMethod]
        public void IsTimeStarted_ForExistingEmployee_WithTimeSpanEnded_ReturnsFalse()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTimeNow);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTimeNow);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTimeNow);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee002Id, dateTimeNow);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTimeNow);

            //

            var timeStarted = portalTimeSpanAccessor.TimeInSpanManagerHelper.IfTimeIsStartedForEmployee(employee001Id);

            Assert.IsFalse(timeStarted);
        }


        [TestMethod]
        public void IsTimeStarted_ForNonExistingEmployee_ThrowsException()
        {
            var dateTimeNow = DateTime.Now;

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTimeNow);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee002Id, dateTimeNow);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTimeNow);

            //

            try
            {
                var timeStarted = portalTimeSpanAccessor.TimeInSpanManagerHelper.IfTimeIsStartedForEmployee(nonExistingEmployeeId);

                Assert.Fail("Exception not thrown! Non existing employee id");
            }
            catch (Exception)
            {

            }
        }

        #endregion

        //

        #region "Advanced Get"


        [TestMethod]
        public void AdvancedGetTimeSpan_With3TimeSpan_OnExistingEmployeeId_WithOffset_WithFetch_ShouldReturnProperOutput()
        {

            var dateTimeNow = DateTime.Now;

            var dateTime01 = dateTimeNow.AddDays(-3);
            var dateTime02 = dateTimeNow.AddDays(-2);
            var dateTime03 = dateTimeNow.AddDays(-1);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTime02);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime03);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee002Id, dateTime02);


            var adGetParam = new AdvancedGetParameters();
            adGetParam.Offset = 1;
            adGetParam.Fetch = 2;


            //

            var list = portalTimeSpanAccessor.TimeInSpanManagerHelper.AdvancedGetTimeInOrOutLogAsList(adGetParam, employee001Id);

            Assert.AreEqual(2, list.Count);
        }



        [TestMethod]
        public void AdvancedGetTimeSpan_With3TimeSpan_OnExistingEmployeeId_WithNoOffset_WithFetch_ShouldReturnProperOutput()
        {

            var dateTimeNow = DateTime.Now;

            var dateTime01 = dateTimeNow.AddDays(-3);
            var dateTime02 = dateTimeNow.AddDays(-2);
            var dateTime03 = dateTimeNow.AddDays(-1);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTime02);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime03);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee002Id, dateTime02);


            var adGetParam = new AdvancedGetParameters();
            //adGetParam.Offset = 1;
            adGetParam.Fetch = 1;


            //

            var list = portalTimeSpanAccessor.TimeInSpanManagerHelper.AdvancedGetTimeInOrOutLogAsList(adGetParam, employee001Id);

            Assert.AreEqual(1, list.Count);
        }



        [TestMethod]
        public void AdvancedGetTimeSpan_With3TimeSpan_OnExistingEmployeeId_WithOffset_WithNoFetch_ShouldReturnProperOutput()
        {

            var dateTimeNow = DateTime.Now;

            var dateTime01 = dateTimeNow.AddDays(-3);
            var dateTime02 = dateTimeNow.AddDays(-2);
            var dateTime03 = dateTimeNow.AddDays(-1);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTime02);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime03);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee002Id, dateTime02);


            var adGetParam = new AdvancedGetParameters();
            adGetParam.Offset = 1;
            //adGetParam.Fetch = 1;


            //

            var list = portalTimeSpanAccessor.TimeInSpanManagerHelper.AdvancedGetTimeInOrOutLogAsList(adGetParam, employee001Id);

            Assert.AreEqual(2, list.Count);
        }



        [TestMethod]
        public void AdvancedGetTimeSpan_With3TimeSpan_OnExistingEmployeeId_WithNoOffset_WithNoFetch_ShouldReturnProperOutput()
        {

            var dateTimeNow = DateTime.Now;

            var dateTime01 = dateTimeNow.AddDays(-3);
            var dateTime02 = dateTimeNow.AddDays(-2);
            var dateTime03 = dateTimeNow.AddDays(-1);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTime02);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime03);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee002Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee002Id, dateTime02);


            var adGetParam = new AdvancedGetParameters();
            //adGetParam.Offset = 1;
            //adGetParam.Fetch = 1;


            //

            var list = portalTimeSpanAccessor.TimeInSpanManagerHelper.AdvancedGetTimeInOrOutLogAsList(adGetParam, employee001Id);

            Assert.AreEqual(3, list.Count);
        }



        [TestMethod]
        public void AdvancedGetTimeSpan_With3TimeSpan_OnNonExistingEmployeeId_ShouldThrowException()
        {

            var dateTimeNow = DateTime.Now;

            var dateTime01 = dateTimeNow.AddDays(-3);
            var dateTime02 = dateTimeNow.AddDays(-2);
            var dateTime03 = dateTimeNow.AddDays(-1);

            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime01);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.EndTimeSpan(employee001Id, dateTime02);
            portalTimeSpanAccessor.TimeInSpanManagerHelper.StartTimeSpan(employee001Id, dateTime03);


            var adGetParam = new AdvancedGetParameters();
            //adGetParam.Offset = 1;
            adGetParam.Fetch = 1;


            //

            try
            {
                var list = portalTimeSpanAccessor.TimeInSpanManagerHelper.AdvancedGetTimeInOrOutLogAsList(adGetParam, employee002Id);

                Assert.Fail("Should throw exception! Employee has no logs here.");
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}
