using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Settings;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.Settings;

internal sealed class ServerSettingEntityRepository(
    SentyllContext ctx
) : EntityRepository<ServerSettingEntity>(ctx), IServerSettingEntityRepository
{

}