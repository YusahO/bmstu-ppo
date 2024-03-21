using MewingPad.Common.Exceptions;

namespace MewingPad.Common.Entities;

public class Score
{
    public Guid AudiofileId { get; set; }
    public Guid AuthorId { get; set; }
    public int Value { get; private set; }

    public Score(Guid audiofileId, Guid authorId, int value)
    {
        AudiofileId = audiofileId;
        AuthorId = authorId;
        Value = (0 <= value && value <= 5) ? value : throw new ScoreInvalidValueException(value);
    }

    public void SetValue(int value)
    {
        if (value < 0 || value > 5)
        {
            throw new ScoreInvalidValueException(value);
        }
        Value = value;
    }

    public Score(Score other)
    {
        AudiofileId = other.AudiofileId;
        AuthorId = other.AuthorId;
        Value = other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Score)
        {
            return false;
        }

        Score other = (Score)obj;
        return other.AuthorId == AuthorId &&
               other.AudiofileId == AudiofileId &&
               other.Value == Value;
    } 
}