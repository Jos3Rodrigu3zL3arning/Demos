using ACME.PayrollManagement.Application.Constants;
using ACME.PayrollManagement.Application.Managers;
using ACME.PayrollManagement.Application.Managers.Implementation;
using ACME.PayrollManagement.Domain.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Application.Tests.Managers
{
    [ExcludeFromCodeCoverage]
    [TestFixture(Category = UNIT_TEST_CATEGORY)]
    [TestOf(nameof(PaymentManager))]
    public class PaymentManagerTests : TestBase
    {
        #region [ SETUP ]

        private IPaymentManager _instanceToTest;

        [OneTimeSetUp]
        public void Setup()
        {
            _instanceToTest = new PaymentManager();
        }

        #endregion [ SETUP ]

        #region [ TESTS ]

        [Test, Order(1)]
        [TestCaseSource(typeof(TestBase), nameof(TEST_CASE_SOURCE_FOR_INVALID_STRINGS))]
        public void ProcessPaymentFilePerWorkerAsync_WhenTheFilePathIsInvalid_ShouldReturnAnArgumentNullException(string filePath)
        {
            // act
            ArgumentNullException? testResult = Assert.ThrowsAsync<ArgumentNullException>(() => _instanceToTest.ProcessPaymentFilePerWorkerAsync(filePath, FILE_SETTINGS));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(MESSAGE_FOR_VALUE_NOT_ALLOWED_AS_NULL), Is.True);
        }

        [Test, Order(2)]
        public void ProcessPaymentFilePerWorkerAsync_WhenTheFilePathDoesNotExist_ShouldReturnAnApplicationException()
        {
            // act
            ApplicationException? testResult = Assert.ThrowsAsync<ApplicationException>(() => _instanceToTest.ProcessPaymentFilePerWorkerAsync(NON_EXISTING_FILE_PATH, FILE_SETTINGS!));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(FileConstants.MESSAGE_FOR_NON_EXISTENT_FILE_DIRECTORY), Is.True);
        }

        [Test, Order(3)]
        public void ProcessPaymentFilePerWorkerAsync_WhenTheFileHasNoContent_ShouldReturnAnApplicationException()
        {
            // act
            ApplicationException? testResult = Assert.ThrowsAsync<ApplicationException>(() => _instanceToTest.ProcessPaymentFilePerWorkerAsync(EMPTY_FILE_PATH, FILE_SETTINGS!));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(FileConstants.MESSAGE_FOR_EMPTY_FILE), Is.True);
        }

        [Test, Order(4)]
        public async Task ProcessPaymentFilePerWorkerAsync_WhenTheFileHasInvalidSeparators_ShouldReturnAnUnsuccessfulResponseAndAnErrorMessage()
        {
            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.ProcessPaymentFilePerWorkerAsync(INVALID_SEPARATORS_FILE_PATH, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.False);
            Assert.That(testResult.Error.Contains(MESSAGE_FOR__FILE_DOES_NOT_HAVE_VALID_SEPARATORS), Is.True);
        }

        [Test, Order(5)]
        public async Task ProcessPaymentFilePerWorkerAsync_WhenTheFileDoesNotHaveAEmployeeNameValid_ShouldReturnAnUnsuccessfulResponseAndAnErrorMessage()
        {
            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.ProcessPaymentFilePerWorkerAsync(NO_EMPLOYEE_NAME_FILE_PATH, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.False);
            Assert.That(testResult.Error.Contains(MESSAGE_FOR_FILE_THAT_DOES_NOT_HAVE_EMPLOYEE_NAME), Is.True);
        }

        [Test, Order(6)]
        public async Task ProcessPaymentFilePerWorkerAsync_WhenTheFileHasEmtpyTimeRanges_ShouldReturnAnUnsuccessfulResponseAndAnErrorMessage()
        {
            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.ProcessPaymentFilePerWorkerAsync(EMPTY_TIME_RANGES_FILE_PATH, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.False);
            Assert.That(testResult.Error.Contains(MESSAGE_FOR_FILE_THAT_DOES_NOT_VALID_TIME_RANGES), Is.True);
        }

        [Test, Order(7)]
        [TestCaseSource(typeof(TestBase), nameof(TEST_CASE_SOURCE_FOR_FILES_WITH_INVALID_DAY_IDENTIFIERS))]
        public async Task ProcessPaymentFilePerWorkerAsync_WhenTheFileDoesNotHaveValidDayIdentifiers_ShouldReturnAnUnsuccessfulResponseAndAnErrorMessage(string filePath)
        {
            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.ProcessPaymentFilePerWorkerAsync(filePath, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.False);
            Assert.That(testResult.Error.Contains(MESSAGE_FOR_FILE_THAT_DOES_NOT_VALID_DAY_IDENTIFIERS), Is.True);
        }

        [Test, Order(8)]
        [TestCaseSource(typeof(TestBase), nameof(TEST_CASE_SOURCE_FOR_FILES_WITH_INVALID_TIME_RANGES))]
        public async Task ProcessPaymentFilePerWorkerAsync_WhenTheFileHasInvalidTimeRanges_ShouldReturnAnUnsuccessfulResponseAndAnErrorMessage(string filePath)
        {
            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.ProcessPaymentFilePerWorkerAsync(filePath, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.False);
            Assert.That(testResult.Error.Contains(MESSAGE_FOR_FILE_THAT_DOES_NOT_VALID_TIME_RANGES), Is.True);
        }

        [Test, Order(9)]
        [TestCaseSource(typeof(TestBase), nameof(TEST_CASE_SOURCE_FOR_FILES_THAT_DONT_HAVE_THE_NUMBER_OF_TIME_RANGES_ALLOWED_PER_DAY))]
        public async Task ProcessPaymentFilePerWorkerAsync_WhenTheFileDoesNotHaveTheRequiredMinimumTimeRangesPerDay_ShouldReturnAnUnsuccessfulResponseAndAnErrorMessage(string filePath)
        {
            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.ProcessPaymentFilePerWorkerAsync(filePath, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.False);
            Assert.That(testResult.Error.Contains(MESSAGE_FOR_FILE_THAT_DOES_NOT_VALID_TIME_RANGES), Is.True);
        }

        [Test, Order(10)]
        [TestCaseSource(typeof(TestBase), nameof(TEST_CASE_SOURCE_FOR_FILES_WITH_INCORRECT_TIME_FORMATS_OR_RANGE_TIMES_OUTSIDE))]
        public async Task ProcessPaymentFilePerWorkerAsync_WhenTheFileHasIncorrectTimeFormatsOrRangesOutsideThoseAllowed_ShouldReturnAnUnsuccessfulResponseAndAnErrorMessage(string filePath)
        {
            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.ProcessPaymentFilePerWorkerAsync(filePath, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.False);
            Assert.That(testResult.Error.Contains(MESSAGE_FOR_FILE_THAT_DOES_NOT_VALID_TIME_RANGES), Is.True);
        }

        [Test, Order(11)]
        [TestCaseSource(typeof(TestBase), nameof(TEST_CASE_SOURCE_FOR_VALID_FILES))]
        public async Task ProcessPaymentFilePerWorkerAsync_WhenThePaymentCalculationIsSuccessful_ShouldReturnASuccessfulResponse(string filePath)
        {
            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.ProcessPaymentFilePerWorkerAsync(filePath, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.True);
            Assert.That(string.IsNullOrEmpty(testResult.Error), Is.True);
            Assert.That(testResult.PaymentDetails, Is.Not.Null);
            Assert.That(testResult.PaymentDetails.Any(), Is.True);
        }

        #endregion [ TESTS ]
    }
}