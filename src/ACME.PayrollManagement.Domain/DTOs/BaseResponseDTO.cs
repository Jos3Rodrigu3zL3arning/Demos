using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ACME.PayrollManagement.Domain.DTOs
{
    [ExcludeFromCodeCoverage]
    public class BaseResponseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public bool Success { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error { get; set; }
    }
}