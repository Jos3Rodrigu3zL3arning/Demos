using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestBase
    {
        #region [ CATEGORIES ]

        public const string UNIT_TEST_CATEGORY = "Unit_Tests";

        #endregion [ CATEGORIES ]

        #region [ SETTINGS ]

        public const string RESOURCES_FILE_PATH = "./Resources/Files/";
        public const string PATH_OF_FILE_TO_UPLOAD = RESOURCES_FILE_PATH + "PaymentFile_20221017.txt";

        #endregion [ SETTINGS ]
    }
}