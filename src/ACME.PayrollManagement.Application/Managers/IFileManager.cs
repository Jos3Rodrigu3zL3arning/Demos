using ACME.PayrollManagement.Domain.DTOs;

namespace ACME.PayrollManagement.Application.Managers
{
    public interface IFileManager
    {
        Task<ProcessedFileResponseDTO> SaveFileAsync(FileToProcessDTO fileToProcess, FileSettingsDTO fileSettings);
    }
}