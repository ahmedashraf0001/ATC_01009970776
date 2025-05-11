namespace event_booking_system.Common.Utils
{
    public class GException : Exception
    {
        public int Code { get;}
        public GException(string message, int statusCode = 500): base(message)
        {
            Code = statusCode;
        }
    }


    public class NotFoundException : GException
    {
        public NotFoundException(string message) : base(message, 404) { }
    }

    public class BadRequestException : GException
    {
        public BadRequestException(string message) : base(message, 400) { }
    }
    public class ConflictException : GException
    {
        public ConflictException(string message) : base(message, 409) { }
    }
    public class ValidationException : GException
    {
        public ValidationException(string message) : base(message, 422) { }
    }
    public class UnauthorizedException : GException
    {
        public UnauthorizedException(string message) : base(message, 401) { }
    }

}
