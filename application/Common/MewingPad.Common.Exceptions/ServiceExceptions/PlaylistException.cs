namespace MewingPad.Common.Exceptions;

public class PlaylistExistsException : BaseException
{
    public PlaylistExistsException() : base() { }
    public PlaylistExistsException(Guid id) : base($"Playlist ID = {id} already exists") { }
    public PlaylistExistsException(string message) : base(message) { }
    public PlaylistExistsException(string message, Exception innerException) : base(message, innerException) { }
    public PlaylistExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class PlaylistNotFoundException : BaseException 
{
    public PlaylistNotFoundException() : base() { }
    public PlaylistNotFoundException(Guid id) : base($"Playlist ID = {id} not found") { }
    public PlaylistNotFoundException(string message) : base(message) { }
    public PlaylistNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public PlaylistNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}