using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Domain.DTOs
{
    [ExcludeFromCodeCoverage]
    public class PaymentPerEmployeeDTO
    {
        public string EmployeeName { get; set; }
        public double TotalToPay { get; set; }
        public string CurrencyType { get; set; }
    }
}