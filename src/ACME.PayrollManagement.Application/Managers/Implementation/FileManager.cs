using ACME.PayrollManagement.Application.Constants;
using ACME.PayrollManagement.Application.Helpers;
using ACME.PayrollManagement.Domain.DTOs;

namespace ACME.PayrollManagement.Application.Managers.Implementation
{
    public class FileManager : IFileManager
    {
        #region [ CONSTANTS ]

        private static readonly string _CURRENT_DATE = DateTime.Now.ToString("yyyyMMdd");

        #endregion [ CONSTANTS ]

        #region [ PUBLIC ]

        public async Task<ProcessedFileResponseDTO> SaveFileAsync(FileToProcessDTO fileToProcess, FileSettingsDTO fileSettings)
        {
            var response = new ProcessedFileResponseDTO();

            if (fileToProcess is null)
            {
                throw new ArgumentNullException(nameof(fileToProcess));
            }

            if (fileSettings is null)
            {
                throw new ArgumentNullException(nameof(fileSettings));
            }

            if (string.IsNullOrEmpty(fileSettings.PathFilesToProcess))
            {
                throw new ArgumentException(ValidationConstants.MESSAGE_FOR_REQUIRED_VALUE, nameof(fileSettings.PathFilesToProcess));
            }

            if (!Directory.Exists(fileSettings.PathFilesToProcess))
            {
                throw new ApplicationException(FileConstants.MESSAGE_FOR_NON_EXISTENT_FILE_DIRECTORY);
            }

            string fileExtension = Path.GetExtension(fileToProcess.File.FileName);

            if (!fileSettings.AllowedFileExtensions.Contains(fileExtension))
            {
                throw new ApplicationException($"File extension '{fileExtension}' is not allowed");
            }

            if (fileToProcess.File.Length == 0)
            {
                throw new ApplicationException(FileConstants.MESSAGE_FOR_EMPTY_FILE);
            }

            string pathToUploadFiles = Path.Combine(fileSettings.PathFilesToProcess, fileSettings.PaymentFolder, _CURRENT_DATE);
            string uniqueFileName = FileHelper.GetUniqueFileName(fileToProcess.File.FileName);
            string filePath = Path.Combine(pathToUploadFiles, uniqueFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileToProcess.File.CopyToAsync(stream);
            }

            response.FilePath = filePath;
            response.Success = true;

            return response;
        }

        #endregion [ PUBLIC ]
    }
}