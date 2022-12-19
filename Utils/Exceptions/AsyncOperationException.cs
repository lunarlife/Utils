namespace Utils.Exceptions;

public class AsyncOperationException : Exception
{
    public AsyncOperationException(string msg) : base(msg)
    {
    }
}