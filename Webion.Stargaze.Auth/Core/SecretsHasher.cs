using System.Security.Cryptography;

namespace Webion.Stargaze.Auth.Core;

public static class SecretsHasher
{
    public static byte[] Hash(byte[] plainSecret)
    {
        return SHA256.HashData(plainSecret);
    }
    
    public static bool Verify(byte[] plainSecret, byte[] hashedSecret)
    {
        return SHA256.HashData(plainSecret).SequenceEqual(hashedSecret);
    }
}