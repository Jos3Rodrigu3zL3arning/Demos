﻿namespace ACME.PayrollManagement.Application.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        { }

        public BadRequestException() : base()
        { }

        public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
        { }
    }
}