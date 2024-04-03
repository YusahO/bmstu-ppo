namespace MewingPad.Common.Exceptions;

public class AudiotrackExistsException : BaseException
{
    public AudiotrackExistsException() : base() { }
    public AudiotrackExistsException(Guid id) : base($"Audiotrack ID = {id} already exists") { }
    public AudiotrackExistsException(string message) : base(message) { }
    public AudiotrackExistsException(string message, Exception innerException) : base(message, innerException) { }
    public AudiotrackExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class AudiotrackNotFoundException : BaseException 
{
    public AudiotrackNotFoundException() : base() { }
    public AudiotrackNotFoundException(Guid id) : base($"Audiotrack ID = {id} not found") { }
    public AudiotrackNotFoundException(string message) : base(message) { }
    public AudiotrackNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public AudiotrackNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}