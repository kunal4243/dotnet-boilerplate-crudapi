using Sodium;

namespace BoilerPlate.Services.Implementations;

public static class PasswordService
{
    public static string HashPassword(string password)
    {
        // Hash the password with Argon2
        var hash = PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Medium);

        return hash;
    }

    public static bool VerifyPassword(string password, string HashPassword)
    {
        return PasswordHash.ArgonHashStringVerify(HashPassword, password);
    }

    public static string GenerateRandomPassword(int length)
    {
        const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-?$%";
        var random = new Random();
        return new string(Enumerable.Repeat(allowedChars, length)
                                    .Select(s => s[random.Next(s.Length)])
                                    .ToArray());
    }


}
