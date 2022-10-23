using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Domain.DTOs
{
    [ExcludeFromCodeCoverage]
    public class FileSettingsDTO
    {
        public string ConfigurationFilesPath { get; set; }
        public string PathFilesToProcess { get; set; }
        public string PaymentFolder { get; set; }
        public string EmployeeNameSeparator { get; set; }
        public string TimeRangeSeparator { get; set; }
        public string HourSeparator { get; set; }
        public string LineSeparator { get; set; }
        public int DayIdentifierStartIndex { get; set; }
        public int DayIdentifierLength { get; set; }
        public IEnumerable<string> AllowedFileExtensions { get; set; }
    }
}