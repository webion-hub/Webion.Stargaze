namespace Webion.ClickUp.Api.V2.Common;

public readonly struct ClickUpId
{
    private long Id { get; init; }

    public ClickUpId(string? id)
    {
        Id = long.TryParse(id, out var res)
            ? res
            : 0;
    }

    public ClickUpId(long id)
    {
        Id = id;
    }

    public static implicit operator ClickUpId(string? id) => new(id);
    public static implicit operator ClickUpId(long id) => new(id);
    
    public static implicit operator string(ClickUpId id) => id.Id.ToString();
    public static implicit operator long(ClickUpId id) => id.Id;

    public override string ToString()
    {
        return this;
    }
}