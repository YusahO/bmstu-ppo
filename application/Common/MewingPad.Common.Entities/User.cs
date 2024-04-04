namespace MewingPad.Common.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid FavouritesId { get; set; }
    public string Username { get; set; }
    public string PasswordHashed { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }

    public User(Guid id, Guid favouritesId, string username, string email, string passwordHashed, bool isAdmin = false)
    {
        Id = id;
        FavouritesId = favouritesId;
        Username = username;
        PasswordHashed = passwordHashed;
        Email = email;
        IsAdmin = isAdmin;
    }

    public User(User other)
    {
        Id = other.Id;
        FavouritesId = other.FavouritesId;
        Username = other.Username;
        PasswordHashed = other.PasswordHashed;
        Email = other.Email;
        IsAdmin = other.IsAdmin;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not User)
        {
            return false;
        }

        User other = (User)obj;
        return Id == other.Id &&
               FavouritesId == other.FavouritesId &&
               Username == other.Username &&
               PasswordHashed == other.PasswordHashed &&
               Email == other.Email &&
               IsAdmin == other.IsAdmin;
    }

    public override int GetHashCode() => base.GetHashCode();
}
