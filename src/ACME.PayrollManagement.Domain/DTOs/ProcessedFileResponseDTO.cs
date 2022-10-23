using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ACME.PayrollManagement.Domain.DTOs
{
    [ExcludeFromCodeCoverage]
    public class ProcessedFileResponseDTO : BaseResponseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<PaymentPerEmployeeDTO> PaymentDetails { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }
    }
}