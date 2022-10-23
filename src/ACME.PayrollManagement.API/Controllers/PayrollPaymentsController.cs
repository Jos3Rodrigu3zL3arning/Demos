using ACME.PayrollManagement.Application.Constants;
using ACME.PayrollManagement.Application.Managers;
using ACME.PayrollManagement.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ACME.PayrollManagement.API.Controllers
{
    public class PayrollPaymentsController : Controller
    {
        private readonly ILogger<PayrollPaymentsController> _logger;
        private readonly IFileManager _fileManager;
        private readonly IPaymentManager _paymentManager;
        private readonly FileSettingsDTO _fileSettings;

        public PayrollPaymentsController(IFileManager fileManager,
                                         IPaymentManager paymentManager,
                                         IOptions<FileSettingsDTO> fileSettings,
                                         ILogger<PayrollPaymentsController> logger)
        {
            _fileManager = fileManager;
            _paymentManager = paymentManager;
            _fileSettings = fileSettings.Value;
            _logger = logger;
        }

        [HttpPost]
        [Route("ProcessFileOfTimesWorkedByEmployee")]
        [RequestSizeLimit(5000)]
        public async Task<IActionResult> ProcessFileOfTimesWorkedByEmployeeAsync([FromForm] FileToProcessDTO fileToProcess)
        {
            var response = new ProcessedFileResponseDTO { Success = false };

            try
            {
                if (fileToProcess is null)
                {
                    response.Error = ResponseConstants.MESSAGE_FOR_INVALID_POST_REQUEST;
                    return BadRequest(response);
                }

                if (fileToProcess.File is null)
                {
                    response.Error = ResponseConstants.MESSAGE_FOR_INVALID_FILE;
                    return BadRequest(response);
                }

                response = await _fileManager.SaveFileAsync(fileToProcess, _fileSettings);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                response = await _paymentManager.ProcessPaymentFilePerWorkerAsync(response.FilePath, _fileSettings);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, FileConstants.ERROR_MESSAGE_DURING_FILE_PROCESSING, args: fileToProcess);
                response.Error = ex.Message;
            }

            return BadRequest(response);
        }
    }
}