using ACME.PayrollManagement.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Domain.DTOs
{
    [ExcludeFromCodeCoverage]
    public class TimeRangesPerEmployeeDTO
    {
        public string EmployeeName { get; set; }
        public DayAbbreviationEnum DayAbbreviation { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public double AmountToPayPerTimeRange { get; set; }
        public CurrencyTypeEnum CurrencyType { get; set; }

        public double DifferenceInMinutes
        {
            get { return (EndTime - StartTime).TotalMinutes; }
        }

        public TimeRangesPerEmployeeDTO(string employeeName)
        {
            EmployeeName = employeeName;
        }
    }
}