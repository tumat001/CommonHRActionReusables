using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHRActionReusables.DateDefinerManager.Utils
{

    /// <summary>
    /// Mainly to do with <see cref="DayWithDefinition.RepeatPerYear"/>. This uses <see cref="DayWithDefinition"/>'s metadata 
    /// fields to give dates based on the given range.
    /// <br/><br/>
    /// Ex: With a DayWithDefinition on Christmas on Repeat, and given a time range of 3 years, it is expected to find
    /// 3 DayWithDefinitions on the output collection.
    /// </summary>
    public class DayWithDefinitionToDaysConverter
    {

        public static IReadOnlyList<DayWithDefinition> GetMetadataAppliedDayWithDefinitions(ICollection<DayWithDefinition> dayWithDefs)
        {
            var bucket = new List<DayWithDefinition>();

            foreach (var item in dayWithDefs)
            {

            }


            return bucket;
        }

    }
}
