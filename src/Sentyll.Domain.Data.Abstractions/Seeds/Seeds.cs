namespace Sentyll.Domain.Data.Abstractions.Seeds;

internal static class Seeds
{
    
    public static ModelBuilder SeedEntities(this ModelBuilder builder)
        => builder
            .SeedServerSettingEntities()
            .SeedEventCategoryEntities();
    
}