namespace MewingPad.Common.Entities;

public class Audiofile
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public float Duration { get; set; }
    public Guid AuthorId { get; set; }
    public string Filepath { get; set; }

    public Audiofile(Guid id, string title, float duration, Guid authorId, string filepath)
    {
        Id = id;
        Title = title;
        Duration = duration;
        AuthorId = authorId;
        Filepath = filepath;
    }

    public Audiofile(Audiofile other)
    {
        Id = other.Id;
        Title = other.Title;
        Duration = other.Duration;
        AuthorId = other.AuthorId;
        Filepath = other.Filepath;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Audiofile)
        {
            return false;
        }

        Audiofile other = (Audiofile)obj;
        return other.Id == Id && 
               other.Title == Title &&
               other.Duration == Duration &&
               other.AuthorId == AuthorId &&
               other.Filepath == Filepath;
    }

    public override int GetHashCode() => base.GetHashCode();
}