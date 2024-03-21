namespace MewingPad.Common.Entities;

public class Commentary
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public Guid AudiofileId { get; set; }
    public string Text { get; set; }

    public Commentary(Guid id, Guid authorId, Guid audiofileId, string text)
    {
        Id = id;
        AuthorId = authorId;
        AudiofileId = audiofileId;
        Text = text;
    }

    public Commentary(Commentary other)
    {
        Id = other.Id;
        AuthorId = other.AuthorId;
        AudiofileId = other.AudiofileId;
        Text = other.Text;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Commentary)
        {
            return false;
        }

        Commentary other = (Commentary)obj;
        return other.Id == Id && 
               other.AuthorId == AuthorId &&
               other.AudiofileId == AudiofileId &&
               other.AuthorId == AuthorId &&
               other.Text == Text;
    } 
}