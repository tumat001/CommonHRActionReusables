using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHRActionReusables.DateDefinerManager;

namespace CommonHRActionReusables.PayrollManager
{
    public class PayrollCalculator
    {

        public double GetCalculatedSalaryFromWorkReportOfDay(HumanWorkReportOfDay reportOfDay, PayRateMatrix payMatrix)
        {
            double amountFromRegularHours = ((double) reportOfDay.regularMinutesPresent / 60.0) * payMatrix.basePayRatePerHour;
            double amountFromOvertimeHours = ((double)reportOfDay.overtimeMinutesPresent / 60.0) * payMatrix.basePayRatePerHour;

            if (reportOfDay.typeOfDay == DayTypes.HOLIDAY)
            {
                amountFromRegularHours += ((double)reportOfDay.regularMinutesPresent / 60.0) * (payMatrix.holidayMultiplier - 1);
                amountFromOvertimeHours += ((double)reportOfDay.overtimeMinutesPresent / 60.0) * (payMatrix.holidayMultiplier - 1);
            }

            return amountFromRegularHours + amountFromOvertimeHours;
        }

        public ResultSummary GetCalculatedSalaryFromWorkReportSummary(HumanWorkReportSummary reportSummary, PayRateMatrix payRateMatrix)
        {
            double totalAmount = 0;
            IDictionary<HumanWorkReportOfDay, double> reportOfDayToPayAmountMap = new Dictionary<HumanWorkReportOfDay, double>();

            foreach (HumanWorkReportOfDay reportOfDay in reportSummary.dateTimeToWorkReportOfDayMap.Values)
            {
                var payAmountOfDay = GetCalculatedSalaryFromWorkReportOfDay(reportOfDay, payRateMatrix);
                reportOfDayToPayAmountMap.Add(reportOfDay, payAmountOfDay);
                totalAmount += payAmountOfDay;
            }

            return new ResultSummary(reportOfDayToPayAmountMap, totalAmount);
        }


        //

        public class ResultSummary
        {

            public IReadOnlyDictionary<HumanWorkReportOfDay, double> workReportOfDayToPayAmount;
            public double totalAmount;

            internal ResultSummary(IDictionary<HumanWorkReportOfDay, double> workReportOfDayToPayAmount, double totalAmount)
            {
                this.workReportOfDayToPayAmount = new Dictionary<HumanWorkReportOfDay, double>(workReportOfDayToPayAmount);
                this.totalAmount = totalAmount;
            }

        }

    }
}
