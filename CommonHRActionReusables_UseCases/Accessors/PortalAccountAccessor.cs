using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.AccountManager;
using CommonDatabaseActionReusables.AccountManager.Actions.Configs;

namespace CommonHRActionReusables_UseCases.Accessors
{
    public class PortalAccountAccessor
    {

        //Change these values upon changing database
        const string EMPLOYEE_DATABASE_CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\CommonHRActionReusables\CommonHRActionReusables_UseCases\Databases\EmployeeDatabase.mdf;Integrated Security=True";
        const string EMPLOYEE_ACCOUNT_TABLE_NAME = "MockEmployeeAccountTable";

        const string EMPLOYEE_ACC_ID_COL_NAME = "Id";
        const string EMPLOYEE_ACC_USERNAME_COL_NAME = "Username";
        const string EMPLOYEE_ACC_PASSWORD_COL_NAME = "Password";
        const string EMPLOYEE_ACC_DISABLED_FROM_LOGIN_COL_NAME = "DisabledFromLogIn";
        const string EMPLOYEE_ACC_EMAIL_COL_NAME = "Email";
        const string EMPLOYEE_ACC_TYPE_COL_NAME = "AccountType";


        public AccountRelatedDatabasePathConfig EmployeeDatabasePathConfig { get; }

        public AccountDatabaseManagerHelper EmployeeAccountManagerHelper { get; }


        public PortalAccountAccessor()
        {
            EmployeeDatabasePathConfig = new AccountRelatedDatabasePathConfig(EMPLOYEE_DATABASE_CONN_STRING, EMPLOYEE_ACC_ID_COL_NAME, EMPLOYEE_ACC_USERNAME_COL_NAME,
                EMPLOYEE_ACC_PASSWORD_COL_NAME, EMPLOYEE_ACC_DISABLED_FROM_LOGIN_COL_NAME, EMPLOYEE_ACC_EMAIL_COL_NAME, EMPLOYEE_ACC_TYPE_COL_NAME, EMPLOYEE_ACCOUNT_TABLE_NAME);

            EmployeeAccountManagerHelper = new AccountDatabaseManagerHelper(EmployeeDatabasePathConfig);
        }

    }
}
