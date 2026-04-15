namespace task4.Helpers;

public class PasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(password))
            return false;

        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}