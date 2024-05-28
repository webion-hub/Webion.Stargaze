using System.Buffers.Text;
using System.Text;
using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Core.Entities;

public readonly record struct ClickUpObjectId(string Id, ClickUpObjectType Type)
{
    public static bool TryDeserialize(string? base64Id, out ClickUpObjectId id)
    {
        id = default;
        if (base64Id is null)
            return false;
        
        var isValidBase64 = Base64.IsValid(base64Id);
        if (!isValidBase64)
            return false;
        
        var bytes = Convert.FromBase64String(base64Id);
        var splitId = Encoding.UTF8.GetString(bytes).Split('_');
        if (splitId.Length != 2)
            return false;
            
        var hasValidType = Enum.TryParse<ClickUpObjectType>(splitId[1], out var idType);
        if (!hasValidType)
            return false;
            
        id = new ClickUpObjectId(splitId[0], idType);
        return true;
    }
    
    public static ClickUpObjectId Deserialize(string base64Id)
    {
        return TryDeserialize(base64Id, out var id)
            ? id
            : throw new InvalidOperationException();
    }
    
    public string Serialize()
    {
        return Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{Id}_{Type}")
        );
    }
}