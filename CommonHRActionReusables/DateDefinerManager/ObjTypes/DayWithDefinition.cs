using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDatabaseActionReusables.GeneralUtilities.TypeUtilities;

namespace CommonHRActionReusables.DateDefinerManager
{
    public class DayWithDefinition : IComparable<DayWithDefinition>, ICloneable
    {

        public int Id { get; }


        public DateTime DateTimeOfDay { get; }

        public string DayTitle { get; }

        public byte[] DayDescription { get; }

        public DayTypes DayType { get; }

        /// <summary>
        /// Is considered a metadata.
        /// </summary>
        public bool RepeatPerYear { get; }


        private DayWithDefinition(DateTime dateTimeOfDay, string dayTitle,
            byte[] dayDescription, DayTypes dayType, bool repeatPerYear, int id)
        {
            DateTimeOfDay = dateTimeOfDay;
            DayTitle = dayTitle;
            DayDescription = dayDescription;
            DayType = dayType;
            RepeatPerYear = repeatPerYear;
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Content as string, assuming that content is assigned with only string.</returns>
        public string GetDescriptionAsString()
        {
            return StringUtilities.ConvertByteArrayToString(DayDescription);
        }

        int IComparable<DayWithDefinition>.CompareTo(DayWithDefinition other)
        {
            return DateTimeOfDay.CompareTo(other.DateTimeOfDay);
        }

        /// <summary>
        /// Applies metadata to this instance and returns a list that represents this instance. All metadata is removed.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<DayWithDefinition> GetMetadataAppliedDayWithDefinitions(DayWithDefinitionParameters param)
        {
            var list = new List<DayWithDefinition>();

            if (RepeatPerYear)
            {
                if (param.DateTimeLowerRange.HasValue && param.DateTimeUpperRange.HasValue)
                {
                    var timespan = param.DateTimeUpperRange - param.DateTimeLowerRange;
                    var daysInTimeSpan = Math.Ceiling(timespan.Value.TotalDays);
                    var yearsInTimeSpan = Math.Ceiling(daysInTimeSpan / 365.25);

                    if (!(DateTimeOfDay.Month > param.DateTimeLowerRange.Value.Month && DateTimeOfDay.Day > param.DateTimeLowerRange.Value.Day))
                    {
                        yearsInTimeSpan -= 1;
                    }

                    for (int i = 0; i < yearsInTimeSpan; i++)
                    {
                        list.Add((DayWithDefinition) Clone());
                    }
                }
                
            }
            else
            {
                list.Add((DayWithDefinition) Clone());
            }

            return list;
        }

        public object Clone()
        {
            return new DayWithDefinition(DateTimeOfDay, DayTitle, DayDescription, DayType, RepeatPerYear, Id);
        }



        //

        public class Builder
        {

            public DateTime DateTimeOfDay { set; get; }

            public string DayTitle { set; get; }

            public byte[] DayDescription { set; get; }

            public DayTypes DayType { set; get; } = DayTypes.REGULAR;

            public bool RepeatPerYear { set; get; } = false;


            public void SetDescriptionUsingString(string descriptionAsString)
            {
                DayDescription = StringUtilities.ConvertStringToByteArray(descriptionAsString);
            }

            public DayWithDefinition build(int id)
            {
                return new DayWithDefinition(DateTimeOfDay, DayTitle, DayDescription, DayType, RepeatPerYear, id);
            }


        }

    }
}
