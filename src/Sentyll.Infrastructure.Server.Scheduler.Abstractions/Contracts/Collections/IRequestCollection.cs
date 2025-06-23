namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Contracts.Collections;

public interface IRequestCollection : IReadOnlyDictionary<
    string, 
    (
        string, 
        Type
    )
>;