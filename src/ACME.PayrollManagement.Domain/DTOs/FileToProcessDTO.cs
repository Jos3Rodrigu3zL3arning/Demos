using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Domain.DTOs
{
    [ExcludeFromCodeCoverage]
    public class FileToProcessDTO
    {
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}