using Asp.Versioning.Conventions;

namespace Sentyll.UI.Startup.Documentation;

/// <summary>
/// 
/// </summary>
internal static class RegisterOpenApiSpecStartupExtensions
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection RegisterOpenApiDocumentation(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSwaggerGen()
            .AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc(options => { options.Conventions.Add(new VersionByNamespaceConvention()); })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

        return serviceCollection;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder applicationBuilder)
        => applicationBuilder
            .UseSwagger()
            .UseSwaggerUI();

}