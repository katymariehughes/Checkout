using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Database
{
    public static class PropertyBuilderExtensions
    {
        public static void HasUtcConversion(this PropertyBuilder<DateTime> property)
           => property.HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    }
}
