using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Extensions;

public static class PropertyBuilderExtensions
{
    public static void HasDefaultTypeIdValue<T>(this PropertyBuilder<T> builder, string typeIdPrefix)
    {
        builder.HasDefaultValueSql($"typeid_generate_text('{typeIdPrefix}')");
    }
}