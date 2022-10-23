using ACME.PayrollManagement.Application.Constants;
using ACME.PayrollManagement.Application.Managers;
using ACME.PayrollManagement.Application.Managers.Implementation;
using ACME.PayrollManagement.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Application.Tests.Managers
{
    [ExcludeFromCodeCoverage]
    [TestFixture(Category = UNIT_TEST_CATEGORY)]
    [TestOf(nameof(FileManager))]
    public class FileManagerTests : TestBase
    {
        #region [ SETUP ]

        #region [ Constants ]

        private static readonly FileToProcessDTO _FILE_TO_PROCESS = new();

        #endregion [ Constants ]

        #region [ Test Case Source ]

        private static readonly object[] _TEST_CASE_SOURCE_FOR_FILES_WITH_EXTENSIONS_NOT_ALLOWED =
        {
            new object[] { "Test.doc" },
            new object[] { "Test.xls" },
            new object[] { "Test.csv" },
            new object[] { "Test.pdf" },
        };

        #endregion [ Test Case Source ]

        private IFileManager _instanceToTest;

        [OneTimeSetUp]
        public void Setup()
        {
            _instanceToTest = new FileManager();
        }

        private static IFormFile GetMockIFormFile(FileInfo physicalFile, bool setFileLength = false)
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
            fileMock.Setup(_ => _.Length).Returns(setFileLength ? ms.Length : 0);
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock.Setup(m => m.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));

            return fileMock.Object;
        }

        private static void SetMockFile(string fileName, string? targetFileFolder = null, bool setFileLenth = false)
        {
            FILE_SETTINGS.PathFilesToProcess = RESOURCES_FILE_PATH + targetFileFolder;
            var physicalFile = new FileInfo(RESOURCES_FILE_PATH + targetFileFolder + fileName);
            _FILE_TO_PROCESS.File = GetMockIFormFile(physicalFile, setFileLenth);
        }

        #endregion [ SETUP ]

        #region [ TESTS ]

        #region [ SaveFileAsync ]

        [Test, Order(1)]
        public void SaveFileAsync_WhenTheFileToProcessInputParameterIsNull_ShouldReturnAnArgumentNullException()
        {
            // arrange
            FileToProcessDTO? fileToProcess = default;

            // act
            ArgumentNullException? testResult = Assert.ThrowsAsync<ArgumentNullException>(() => _instanceToTest.SaveFileAsync(fileToProcess!, FILE_SETTINGS));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(MESSAGE_FOR_VALUE_NOT_ALLOWED_AS_NULL), Is.True);
        }

        [Test, Order(2)]
        public void SaveFileAsync_WhenTheFileSettingsInputParameterIsNull_ShouldReturnAnArgumentNullException()
        {
            // arrange
            FileSettingsDTO? fileSettings = default;

            // act
            ArgumentNullException? testResult = Assert.ThrowsAsync<ArgumentNullException>(() => _instanceToTest.SaveFileAsync(_FILE_TO_PROCESS, fileSettings!));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(MESSAGE_FOR_VALUE_NOT_ALLOWED_AS_NULL), Is.True);
        }

        [Test, Order(3)]
        [TestCaseSource(typeof(TestBase), nameof(TEST_CASE_SOURCE_FOR_INVALID_STRINGS))]
        public void SaveFileAsync_WhenThePathFilesToProcessInputParameterIsInvalid_ShouldReturnAnArgumentException(string pathFilesToProcess)
        {
            // arrange
            FILE_SETTINGS.PathFilesToProcess = pathFilesToProcess;

            // act
            ArgumentException? testResult = Assert.ThrowsAsync<ArgumentException>(() => _instanceToTest.SaveFileAsync(_FILE_TO_PROCESS, FILE_SETTINGS!));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(ValidationConstants.MESSAGE_FOR_REQUIRED_VALUE), Is.True);
        }

        [Test, Order(4)]
        public void SaveFileAsync_WhenThePathToSaveFileDoesNotExist_ShouldReturnAnApplicationException()
        {
            // arrange
            FILE_SETTINGS.PathFilesToProcess = NON_EXISTING_FILE_PATH;

            // act
            ApplicationException? testResult = Assert.ThrowsAsync<ApplicationException>(() => _instanceToTest.SaveFileAsync(_FILE_TO_PROCESS, FILE_SETTINGS!));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(FileConstants.MESSAGE_FOR_NON_EXISTENT_FILE_DIRECTORY), Is.True);
        }

        [Test, Order(5)]
        [TestCaseSource(nameof(_TEST_CASE_SOURCE_FOR_FILES_WITH_EXTENSIONS_NOT_ALLOWED))]
        public void SaveFileAsync_WhenTheFileToProcessHasAnExtensionNotAllowed_ShouldReturnAnApplicationException(string fileName)
        {
            // arrange
            SetMockFile(fileName, FOLDER_PATH_FOR_DISALLOWED_EXTENSIONS);

            // act
            ApplicationException? testResult = Assert.ThrowsAsync<ApplicationException>(() => _instanceToTest.SaveFileAsync(_FILE_TO_PROCESS, FILE_SETTINGS!));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.Contains(Path.GetExtension(fileName)), Is.True);
        }

        [Test, Order(6)]
        public void SaveFileAsync_WhenTheFileToProcessHasNoContent_ShouldReturnAnArgumentException()
        {
            // arrange
            SetMockFile(EMPTY_FILE_NAME);

            // act
            ApplicationException? testResult = Assert.ThrowsAsync<ApplicationException>(() => _instanceToTest.SaveFileAsync(_FILE_TO_PROCESS, FILE_SETTINGS!));

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Message.StartsWith(FileConstants.MESSAGE_FOR_EMPTY_FILE), Is.True);
        }

        [Test, Order(7)]
        public async Task SaveFileAsync_WhenTheFileToProcessIsLoadedCorrectly_ShouldReturnSuccesfullResponse()
        {
            // arrange
            SetMockFile(FILE_PATH_WITH_VALID_CONTENT, setFileLenth: true);

            // act
            ProcessedFileResponseDTO testResult = await _instanceToTest.SaveFileAsync(_FILE_TO_PROCESS, FILE_SETTINGS!);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult.Success, Is.True);
            Assert.That(string.IsNullOrEmpty(testResult.FilePath), Is.False);
            Assert.That(testResult.FilePath.Contains(Path.GetFileNameWithoutExtension(FILE_PATH_WITH_VALID_CONTENT)), Is.True);
            Assert.That(testResult.FilePath.Contains(Path.GetExtension(FILE_PATH_WITH_VALID_CONTENT)), Is.True);
        }

        #endregion [ SaveFileAsync ]

        #endregion [ TESTS ]
    }
}