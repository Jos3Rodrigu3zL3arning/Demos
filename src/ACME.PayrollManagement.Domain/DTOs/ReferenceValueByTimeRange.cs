using ACME.PayrollManagement.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Domain.DTOs
{
    [ExcludeFromCodeCoverage]
    public class ReferenceValueByTimeRange
    {
        public DayAbbreviationEnum Day { get; set; }
        public string StartTimeStr { get; set; }
        public string EndTimeStr { get; set; }
        public decimal ValueToPayPerHour { get; set; }
        public CurrencyTypeEnum CurrencyType { get; set; }

        public TimeOnly StartTime
        {
            get { return TimeOnly.Parse(StartTimeStr); }
        }

        public TimeOnly EndTime
        {
            get { return TimeOnly.Parse(EndTimeStr); }
        }
    }
}