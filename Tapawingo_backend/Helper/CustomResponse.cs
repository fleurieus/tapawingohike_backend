namespace Tapawingo_backend
{
    public class CustomResponse
    {
        public string Message { get; }

        public CustomResponse( string message = null)
        {
            Message = message;
        }
    }

    public enum Status
    {
        Success,
        UnProcessableEntity,
    }
}
