using ACME.PayrollManagement.Application.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Application.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    [TestFixture(Category = UNIT_TEST_CATEGORY)]
    [TestOf(nameof(FileHelper))]
    public class FileHelperTests : TestBase
    {
        #region [ SETUP ]

        private static readonly object[] _TEST_CASE_SOURCE_FOR_VALID_FILE_NAMES =
        {
            new object[] { "Filename1.txt" },
            new object[] { "Test.txt" },
        };

        [OneTimeSetUp]
        public void Setup()
        { }

        #endregion [ SETUP ]

        #region [ TESTS ]

        [Test, Order(1)]
        [TestCaseSource(typeof(TestBase), nameof(TEST_CASE_SOURCE_FOR_INVALID_STRINGS))]
        public void GetUniqueFileName_WhenTheInputParameterForFileNameIsInvalid_ShouldReturnAnArgumentNullException(string fileName)
        {
            // act
            ArgumentNullException? testResult = Assert.Throws<ArgumentNullException>(() => FileHelper.GetUniqueFileName(fileName));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(MESSAGE_FOR_VALUE_NOT_ALLOWED_AS_NULL), Is.True);
        }

        [Test, Order(2)]
        [TestCaseSource(nameof(_TEST_CASE_SOURCE_FOR_VALID_FILE_NAMES))]
        public void GetUniqueFileName_WhenTheInputParameterForFileNameIsValid_ShouldReturnTheNewFileName(string fileName)
        {
            // act
            string testResult = FileHelper.GetUniqueFileName(fileName);

            // assert
            Assert.That(string.IsNullOrEmpty(testResult), Is.False);
            Assert.That(testResult.StartsWith(Path.GetFileNameWithoutExtension(fileName)), Is.True);
        }

        #endregion [ TESTS ]
    }
}