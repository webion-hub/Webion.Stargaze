using System.ComponentModel.DataAnnotations.Schema;
using FastIDs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities;

public interface IEntityBase
{
    [NotMapped]
    public string IdPrefix { get; }

    public static TypeId New<T>() where T: IEntityBase, new()
    {
        return TypeId
            .New(new T().IdPrefix, false)
            .Encode();
    }
}