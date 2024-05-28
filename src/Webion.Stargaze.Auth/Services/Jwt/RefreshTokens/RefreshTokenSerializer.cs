namespace Webion.Stargaze.Auth.Services.Jwt.RefreshTokens;

public static class RefreshTokenSerializer
{
    public static string Serialize(Guid id, byte[] secret)
    {
        return $"{id}_{Convert.ToBase64String(secret)}";
    }

    public static bool TryDeserialize(string refreshToken, out RefreshToken result)
    {
        result = default;
        
        var split = refreshToken.Split('_');
        if (split.Length != 2)
            return false;
        
        if (!Guid.TryParse(split[0], out var id))
            return false;

        var secret = new byte[RefreshToken.SecretSize];
        if (!Convert.TryFromBase64String(split[1], secret, out _))
            return false;
        
        result = new RefreshToken(id, secret);
        return true;
    }
}