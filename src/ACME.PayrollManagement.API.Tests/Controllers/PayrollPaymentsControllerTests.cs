using ACME.PayrollManagement.API.Controllers;
using ACME.PayrollManagement.Application.Managers;
using ACME.PayrollManagement.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.API.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestFixture(Category = UNIT_TEST_CATEGORY)]
    [TestOf(nameof(PayrollPaymentsController))]
    public class PayrollPaymentsControllerTests : TestBase
    {
        #region [ SETUP ]

        #region [ Constants ]

        private static readonly FileToProcessDTO _FILE_TO_PROCESS = new();

        private static readonly ProcessedFileResponseDTO _SUCCESSFUL_RESPONSE = new()
        {
            Success = true,
            FilePath = "c://PaymentsFile.txt",
            PaymentDetails = new List<PaymentPerEmployeeDTO>
            {
                new() { EmployeeName = "Employee 1", TotalToPay = 10, CurrencyType = "US" },
                new() { EmployeeName = "Employee 2", TotalToPay = 20, CurrencyType = "US" },
            }
        };

        private static readonly ProcessedFileResponseDTO _UNSUCCESSFUL_RESPONSE = new()
        {
            Success = false,
            Error = "ErrorMessage"
        };

        #endregion [ Constants ]

        #region [ Properties ]

        private bool FileUploadedSuccessfully { get; set; }
        private bool PaymentsCalculatedCorrectly { get; set; }

        #endregion [ Properties ]

        private PayrollPaymentsController _instanceToTest;

        [OneTimeSetUp]
        public void Setup()
        {
            var fileManagerMock = new Mock<IFileManager>();
            FileManagerSetup(fileManagerMock);

            var paymentManagerMock = new Mock<IPaymentManager>();
            PaymentManagerSetup(paymentManagerMock);

            var optionsMock = new Mock<IOptions<FileSettingsDTO>>();
            var loggerMock = new Mock<ILogger<PayrollPaymentsController>>();

            _instanceToTest = new PayrollPaymentsController(fileManagerMock.Object,
                                                            paymentManagerMock.Object,
                                                            optionsMock.Object,
                                                            loggerMock.Object);

            SetMockFile();
        }

        private void FileManagerSetup(Mock<IFileManager> fileManagerMock)
        {
            fileManagerMock
                .Setup(x => x.SaveFileAsync(It.IsAny<FileToProcessDTO>(), It.IsAny<FileSettingsDTO>()))
                .ReturnsAsync(() => FileUploadedSuccessfully ? _SUCCESSFUL_RESPONSE : _UNSUCCESSFUL_RESPONSE);
        }

        private void PaymentManagerSetup(Mock<IPaymentManager> paymentManagerMock)
        {
            paymentManagerMock
                .Setup(x => x.ProcessPaymentFilePerWorkerAsync(It.IsAny<string>(), It.IsAny<FileSettingsDTO>()))
                .ReturnsAsync(() => PaymentsCalculatedCorrectly ? _SUCCESSFUL_RESPONSE : _UNSUCCESSFUL_RESPONSE);
        }

        private static IFormFile GetMockIFormFile(FileInfo physicalFile)
        {
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            string fileName = physicalFile.Name;
            //Setup mock file using info from physical file
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock.Setup(m => m.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));

            return fileMock.Object;
        }

        private static void SetMockFile()
        {
            var physicalFile = new FileInfo(PATH_OF_FILE_TO_UPLOAD);
            _FILE_TO_PROCESS.File = GetMockIFormFile(physicalFile);
        }

        #endregion [ SETUP ]

        #region [ TESTS ]

        #region [ ProcessFileOfTimesWorkedByEmployee ]

        [Test, Order(1)]
        public async Task ProcessFileOfTimesWorkedByEmployeeAsync_WhenTheFileToProcessInputParameterIsNull_ShouldReturnABadRequesObjectResult()
        {
            // arrange
            FileToProcessDTO? fileToProcess = default;

            // act
            IActionResult testResult = await _instanceToTest.ProcessFileOfTimesWorkedByEmployeeAsync(fileToProcess!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.InstanceOf(typeof(BadRequestObjectResult)));
        }

        [Test, Order(2)]
        public async Task ProcessFileOfTimesWorkedByEmployeeAsync_WhenTheFileToLoadIsNull_ShouldReturnABadRequesObjectResult()
        {
            // act
            IActionResult testResult = await _instanceToTest.ProcessFileOfTimesWorkedByEmployeeAsync(_FILE_TO_PROCESS);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.InstanceOf(typeof(BadRequestObjectResult)));
        }

        [Test, Order(3)]
        public async Task ProcessFileOfTimesWorkedByEmployeeAsync_WhenSavingTheFileFails_ShouldReturnABadRequesObjectResult()
        {
            // arrange
            FileUploadedSuccessfully = false;

            // act
            IActionResult testResult = await _instanceToTest.ProcessFileOfTimesWorkedByEmployeeAsync(_FILE_TO_PROCESS);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.InstanceOf(typeof(BadRequestObjectResult)));
        }

        [Test, Order(4)]
        public async Task ProcessFileOfTimesWorkedByEmployeeAsync_WhenThePaymentCalculationFails_ShouldReturnABadRequesObjectResult()
        {
            // arrange
            FileUploadedSuccessfully = true;
            PaymentsCalculatedCorrectly = false;

            // act
            IActionResult testResult = await _instanceToTest.ProcessFileOfTimesWorkedByEmployeeAsync(_FILE_TO_PROCESS);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.InstanceOf(typeof(BadRequestObjectResult)));
        }

        [Test, Order(5)]
        public async Task ProcessFileOfTimesWorkedByEmployeeAsync_WhenThePaymentCalculationSuccessful_ShouldReturnAnOkObjectResult()
        {
            // arrange
            FileUploadedSuccessfully = true;
            PaymentsCalculatedCorrectly = true;

            // act
            IActionResult testResult = await _instanceToTest.ProcessFileOfTimesWorkedByEmployeeAsync(_FILE_TO_PROCESS);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.InstanceOf(typeof(OkObjectResult)));
        }

        #endregion [ ProcessFileOfTimesWorkedByEmployee ]

        #endregion [ TESTS ]
    }
}