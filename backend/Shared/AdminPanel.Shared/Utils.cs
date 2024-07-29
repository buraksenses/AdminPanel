using System.Security.Cryptography;

namespace AdminPanel.Shared;

public static class Utils
{
    public static string GenerateSecureJwtKey(int size = 32)
    {
        var key = new byte[size];
        RandomNumberGenerator.Fill(key);
        return Convert.ToBase64String(key);
    }
}