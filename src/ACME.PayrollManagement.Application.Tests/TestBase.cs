using ACME.PayrollManagement.Domain.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestBase
    {
        #region [ CATEGORIES ]

        public const string UNIT_TEST_CATEGORY = "Unit_Tests";

        #endregion [ CATEGORIES ]

        #region [ SETTINGS ]

        public const string RESOURCES_FILE_PATH = "./Resources/Files/";
        public const string FOLDER_PATH_FOR_DISALLOWED_EXTENSIONS = "ExtensionsNotAllowed/";
        public static readonly string NON_EXISTING_FILE_PATH = $"c://{DateTime.Now:yyyyMMddHHmmss}";
        public const string EMPTY_FILE_NAME = "/EmptyFiles/EmptyFile.txt";
        public const string EMPTY_FILE_PATH = RESOURCES_FILE_PATH + "/EmptyFiles/EmptyFile.txt";
        public const string INVALID_SEPARATORS_FILE_PATH = RESOURCES_FILE_PATH + "/InvalidSeparators/FileWithInvalidSeparators.txt";
        public const string NO_EMPLOYEE_NAME_FILE_PATH = RESOURCES_FILE_PATH + "/NoEmployeeName/FileWithoutEmployeeName.txt";
        public const string EMPTY_TIME_RANGES_FILE_PATH = RESOURCES_FILE_PATH + "/EmptyTimeRanges/FileWithEmptyTimeRanges.txt";
        public const string FILE_PATH_WITH_VALID_CONTENT = "/ValidFiles/PaymentFile_20221017.txt";

        public static readonly FileSettingsDTO FILE_SETTINGS = new()
        {
            ConfigurationFilesPath = RESOURCES_FILE_PATH + "/ConfigurationFiles/ReferenceValuesByTimeRange.json",
            PathFilesToProcess = "F:\\Temporal",
            PaymentFolder = "Payments",
            EmployeeNameSeparator = "=",
            TimeRangeSeparator = ",",
            HourSeparator = "-",
            LineSeparator = "\r\n",
            DayIdentifierStartIndex = 0,
            DayIdentifierLength = 2,
            AllowedFileExtensions = new List<string> { ".txt" }
        };

        private const string _FILE_FOLDER_WITH_INVALID_DAY_IDENTIFIERS = "InvalidDayIdentifiers";
        private const string _FILE_FOLDER_WITH_INVALID_TIME_RANGES = "InvalidTimeRanges";
        private const string _FILE_FOLDER_THAT_DONT_MEET_NUMBER_OF_TIME_RANGES_PER_DAY = "DontMeetNumberOfTimeRangesPerDay";
        private const string _FILE_FOLDER_WITH_VALID_FILES = "ValidFiles";

        #endregion [ SETTINGS ]

        #region [ MESSAGES ]

        public const string MESSAGE_FOR_VALUE_NOT_ALLOWED_AS_NULL = "Value cannot be null.";
        public const string MESSAGE_FOR__FILE_DOES_NOT_HAVE_VALID_SEPARATORS = "file does not have valid separators.";
        public const string MESSAGE_FOR_FILE_THAT_DOES_NOT_HAVE_EMPLOYEE_NAME = "doesn't have a valid employee name.";
        public const string MESSAGE_FOR_FILE_THAT_DOES_NOT_VALID_TIME_RANGES = "has no valid time ranges.";
        public const string MESSAGE_FOR_FILE_THAT_DOES_NOT_VALID_DAY_IDENTIFIERS = "has no valid day identifiers.";

        #endregion [ MESSAGES ]

        #region [ TEST CASE SOURCE ]

        public static readonly object[] TEST_CASE_SOURCE_FOR_INVALID_STRINGS =
        {
            new object[] { null! },
            new object[] { string.Empty },
            new object[] { "" }
        };

        public static string[] TEST_CASE_SOURCE_FOR_FILES_WITH_INVALID_DAY_IDENTIFIERS
        {
            get
            {
                return GetListOfFiles(_FILE_FOLDER_WITH_INVALID_DAY_IDENTIFIERS);
            }
        }

        public static string[] TEST_CASE_SOURCE_FOR_FILES_WITH_INVALID_TIME_RANGES
        {
            get
            {
                return GetListOfFiles(_FILE_FOLDER_WITH_INVALID_TIME_RANGES);
            }
        }

        public static string[] TEST_CASE_SOURCE_FOR_FILES_THAT_DONT_HAVE_THE_NUMBER_OF_TIME_RANGES_ALLOWED_PER_DAY
        {
            get
            {
                return GetListOfFiles(_FILE_FOLDER_THAT_DONT_MEET_NUMBER_OF_TIME_RANGES_PER_DAY);
            }
        }

        public static string[] TEST_CASE_SOURCE_FOR_FILES_WITH_INCORRECT_TIME_FORMATS_OR_RANGE_TIMES_OUTSIDE
        {
            get
            {
                return GetListOfFiles(_FILE_FOLDER_WITH_INVALID_TIME_RANGES);
            }
        }

        public static string[] TEST_CASE_SOURCE_FOR_VALID_FILES
        {
            get
            {
                return GetListOfFiles(_FILE_FOLDER_WITH_VALID_FILES);
            }
        }

        private static string[] GetListOfFiles(string folderWithTestFiles)
        {
            return Directory.GetFiles(RESOURCES_FILE_PATH + "/" + folderWithTestFiles + "/");
        }

        #endregion [ TEST CASE SOURCE ]
    }
}