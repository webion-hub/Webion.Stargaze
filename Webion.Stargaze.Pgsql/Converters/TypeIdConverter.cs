using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Converters;

public sealed class TypeIdConverter : ValueConverter<TypeId, string>
{
    public TypeIdConverter()
        : base(x => x.ToString(), x => TypeId.Parse(x))
    {}
}