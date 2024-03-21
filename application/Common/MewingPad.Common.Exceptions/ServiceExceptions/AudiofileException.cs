namespace MewingPad.Common.Exceptions;

public class AudiofileExistsException : BaseException
{
    public AudiofileExistsException() : base() { }
    public AudiofileExistsException(Guid id) : base($"Audiofile ID = {id} already exists") { }
    public AudiofileExistsException(string message) : base(message) { }
    public AudiofileExistsException(string message, Exception innerException) : base(message, innerException) { }
    public AudiofileExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class AudiofileNotFoundException : BaseException 
{
    public AudiofileNotFoundException() : base() { }
    public AudiofileNotFoundException(Guid id) : base($"Audiofile ID = {id} not found") { }
    public AudiofileNotFoundException(string message) : base(message) { }
    public AudiofileNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public AudiofileNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}