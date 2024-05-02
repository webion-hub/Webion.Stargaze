using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Extensions;

public static class TableBuilderExtensions
{
    public static void HasTypeIdCheckConstraint<T>(this TableBuilder<T> builder,
        string typeIdPrefix,
        string fieldName = "id"
    )
        where T: class
    {
        builder.HasCheckConstraint(
            name: $"CK_{fieldName}",
            sql: $"typeid_check_text({fieldName}, '{typeIdPrefix}')"
        );
    }
}