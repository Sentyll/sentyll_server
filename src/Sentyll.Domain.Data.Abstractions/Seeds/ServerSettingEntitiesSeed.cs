using Sentyll.Domain.Data.Abstractions.Constants.Seeds.Settings;
using Sentyll.Domain.Data.Abstractions.Entities.Settings;

namespace Sentyll.Domain.Data.Abstractions.Seeds;

internal static class ServerSettingEntitiesSeed
{
    public static ModelBuilder SeedServerSettingEntities(this ModelBuilder builder)
    {
        builder.Entity<ServerSettingEntity>()
            .HasData(
                ServerSettingEntitiesConstants.MaxActivateApiRequests,
                ServerSettingEntitiesConstants.MinimumSecondsBetweenFailureNotifications,
                ServerSettingEntitiesConstants.NotifyUnHealthyOneTimeUntilChange,
                ServerSettingEntitiesConstants.ExecutionsCanExpire,
                ServerSettingEntitiesConstants.MaxExecutionsRetentionPeriodInDays,
                ServerSettingEntitiesConstants.ExecutionHistoriesCanExpire,
                ServerSettingEntitiesConstants.MaxExecutionHistoriesRetentionPeriodInDays
            );

        return builder;
    }
}