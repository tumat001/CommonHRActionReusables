using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.TimeInSpanManager
{
    public class TimeSpanOfWork
    {

        public readonly DateTime dateTimeOfStart;
        public readonly DateTime dateTimeOfEnd;

        private bool isDateOfStartSet;
        private bool isDateOfEndSet;

        public TimeSpanOfWork(DateTime dateTimeOfStart, DateTime dateTimeOfEnd)
        {
            this.dateTimeOfStart = dateTimeOfStart;
            this.dateTimeOfEnd = dateTimeOfEnd;

            isDateOfStartSet = true;
            isDateOfEndSet = true;
        }

        public TimeSpanOfWork(DateTime dateTimeOfStart)
        {
            this.dateTimeOfStart = dateTimeOfStart;

            isDateOfStartSet = true;
            isDateOfEndSet = true;
        }


        /// <summary>
        /// Returns the number of minutes between <see cref="dateTimeOfStart"/> and <see cref="dateTimeOfEnd"/>.<br></br>
        /// Returns -1 if there is no time of end.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfMinutesInThisTimeSpan()
        {
            if (isDateOfEndSet)
            {
                return (int)Math.Round((dateTimeOfEnd - dateTimeOfStart).TotalMinutes);
            }
            else
            {
                return -1;
            }
        }

        public bool HasDateTimeOfEnd()
        {
            return isDateOfEndSet == true;
        }

    }
}
