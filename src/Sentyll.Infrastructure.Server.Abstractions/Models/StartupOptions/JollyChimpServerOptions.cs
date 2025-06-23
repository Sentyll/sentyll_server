namespace Sentyll.Infrastructure.Server.Abstractions.Models.StartupOptions;

public sealed record SentyllServerOptions
{

    /// <summary>
    /// SQL Server Connection string used to store API results for history tracking
    /// </summary>
    public string ConnectionString { get; set; }
    
    public bool DisableMigrations { get; set; } = false;
    
    public string EncryptionKey { get; set; }
}