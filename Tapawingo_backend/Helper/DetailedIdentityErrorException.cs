namespace Tapawingo_backend
{
    public class DetailedIdentityErrorException : Exception
    {
        public Status Status { get; }
        public string Message { get; }

        public DetailedIdentityErrorException(Status status, string message) : base(message)
        {
            Status = status;
            Message = message;
        }

    }

}
