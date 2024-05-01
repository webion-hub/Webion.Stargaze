using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities;

public interface IEntity
{
    public TypeId Id { get; set; }
    public string GetIdPrefix();
}