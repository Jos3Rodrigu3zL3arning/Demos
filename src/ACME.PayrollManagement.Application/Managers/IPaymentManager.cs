using ACME.PayrollManagement.Domain.DTOs;

namespace ACME.PayrollManagement.Application.Managers
{
    public interface IPaymentManager
    {
        Task<ProcessedFileResponseDTO> ProcessPaymentFilePerWorkerAsync(string filePath, FileSettingsDTO fileSettings);
    }
}