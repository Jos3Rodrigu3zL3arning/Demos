using ACME.PayrollManagement.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Application.Constants
{
    [ExcludeFromCodeCoverage]
    public static class FileConstants
    {
        #region [ SETTINGS ]

        public const int MINUTES_PER_HOUR = 60;
        public const int NUMBER_OF_HOURS_ALLOWED_PER_DAY = 2;
        public const string MESSAGE_FOR_HOUR_RANGE_NOT_VALID = "The hour range is not valid.";
        public const string MESSAGE_FOR_WRONG_NUMBER_OF_TIME_RANGES_PER_DAY = "Only one start and end time is allowed for each period.";
        public const string MESSAGE_FOR_INVALID_START_TIME = "The start time is invalid.";
        public const string MESSAGE_FOR_INVALID_END_TIME = "The end time is invalid.";
        public const string MESSAGE_FOR_START_TIME_GREATER_THAN_END_TIME = "The start time must be less than the end time.";
        public const string MESSAGE_FOR_INVALID_TIME_RANGE = "Please note that only these time ranges are allowed per day -";

        public static readonly IReadOnlyCollection<string> ALLOWED_DAY_ABBREVIATIONS = new List<string>
        {
            DayAbbreviationEnum.MO.ToString(),
            DayAbbreviationEnum.TU.ToString(),
            DayAbbreviationEnum.WE.ToString(),
            DayAbbreviationEnum.TH.ToString(),
            DayAbbreviationEnum.FR.ToString(),
            DayAbbreviationEnum.SA.ToString(),
            DayAbbreviationEnum.SU.ToString(),
        }.AsReadOnly();

        public static readonly IReadOnlyCollection<(TimeOnly, TimeOnly)> ALLOWED_TIME_RANGES = new List<(TimeOnly, TimeOnly)>
        {
            (new TimeOnly(0,0,0), new TimeOnly(9,0,59)),
            (new TimeOnly(9,1,0), new TimeOnly(18,0,59)),
            (new TimeOnly(18,1,0), new TimeOnly(23,59,59)),
        }.AsReadOnly();

        #endregion [ SETTINGS ]

        #region [ MESSAGES ]

        public const string MESSAGE_FOR_NON_EXISTENT_FILE_DIRECTORY = "The path to save the files does not exist.";
        public const string MESSAGE_FOR_EMPTY_FILE = "The file is empty.";
        public const string MESSAGE_FOR_INVALID_NAMES_IN_FILE = "No valid employee names were found in the file.";
        public const string MESSAGE_FOR_INVALID_FILE_CONTENT = "The file has errors in the content and could not be processed.";
        public const string MESSAGE_FOR_CONFIGURATION_FILE_NOT_FOUND = "Could not find the configuration file to calculate the values. Contact the administrator.";
        public const string ERROR_MESSAGE_DURING_FILE_PROCESSING = "An error occurred while processing the file.";
        public const string ERROR_MESSAGE_FOR_INVALID_PAYMENT_SETTINS = "No valid configuration was found to perform the payment calculation.";

        #endregion [ MESSAGES ]
    }
}