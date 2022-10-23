using System.Diagnostics.CodeAnalysis;

namespace ACME.PayrollManagement.Application.Constants
{
    [ExcludeFromCodeCoverage]
    public static class ResponseConstants
    {
        public const string MESSAGE_FOR_INVALID_POST_REQUEST = "Invalid post request";
        public const string MESSAGE_FOR_INVALID_FILE = "The file is not valid";
    }
}