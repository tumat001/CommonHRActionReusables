using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.TimeInSpanManager.Utils
{
    public struct TimeLogToTimeSpanConverter
    {

        public IReadOnlyList<TimeSpanOfWork> ConvertTimeLogsToTimeSpans(ICollection<TimeInOrOutLog> logs)
        {
            var list = new List<TimeSpanOfWork>();

            TimeSpanOfWork currTimeSpan;
            DateTime dateTimeOfStart = DateTime.Now; //this value will not be used
            bool isStartOfConstruction = false;

            foreach (TimeInOrOutLog log in logs)
            {

                var isStart = log.IsTimeIn;

                if (isStart)
                {
                    dateTimeOfStart = log.DateTimeOfLog;
                    isStartOfConstruction = true;
                }
                else
                {
                    var dateTimeOfEnd = log.DateTimeOfLog;
                    currTimeSpan = new TimeSpanOfWork(dateTimeOfStart, dateTimeOfEnd);

                    list.Add(currTimeSpan);

                    currTimeSpan = null;
                    isStartOfConstruction = false;
                }


            }

            if (isStartOfConstruction)
            {
                currTimeSpan = new TimeSpanOfWork(dateTimeOfStart);

                list.Add(currTimeSpan);
            }


            return list;
        }


    }
}
