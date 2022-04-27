using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.PayrollManager
{
    public class PayRateMatrix
    {

        public readonly double regularHoursMultiplier; //standard multiplier.

        public readonly double overtimeHoursMultiplier;
        public readonly double holidayMultiplier;

        public readonly double basePayRatePerHour;


        public PayRateMatrix(double basePayRatePerHour, double regularHoursMultiplier,
            double overtimeHoursMultiplier, double holidayMultiplier)
        {
            this.basePayRatePerHour = basePayRatePerHour;
            this.regularHoursMultiplier = regularHoursMultiplier;
            this.overtimeHoursMultiplier = overtimeHoursMultiplier;
            this.holidayMultiplier = holidayMultiplier;
        }


    }
}
