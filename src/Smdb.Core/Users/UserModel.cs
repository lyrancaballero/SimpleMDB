namespace Smdb.Core.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }   // plain text for simplicity
    public string Role { get; set; }       // "user" or "admin"

    public User(int id, string username, string password, string role)
    {
        Id = id;
        Username = username;
        Password = password;
        Role = role;
    }

    public override string ToString()
    {
        return $"User[Id={Id}, Username={Username}, Role={Role}]";
    }
}
