using System.Globalization;
using System.Text.Json.Nodes;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Constants;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Options;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Request;

namespace Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Builders;

internal sealed class MicrosoftTeamsRestContentBuilder
{
    private readonly JsonArray _rootJson;
    
    private readonly ServerEndpointsOptions _serverEndpointsOptions;
    
    private MicrosoftTeamsRestContentBuilder(
        ServerEndpointsOptions serverEndpointsOptions
    )
    {
        _serverEndpointsOptions = serverEndpointsOptions;
        _rootJson = new();
    }

    public static MicrosoftTeamsRestContentBuilder Init(ServerEndpointsOptions serverEndpointsOptions)
        => new(serverEndpointsOptions);

    public MicrosoftTeamsRestContentBuilder WithDivider()
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "TextBlock",
            ["text"] = "---",
            ["separator"] = true
        });
        
        return this;
    }
    
    public MicrosoftTeamsRestContentBuilder WithFailureHeading(int failureCount)
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "TextBlock",
            ["text"] = $"**{NotificationVerbiageConstants.HEADLINE_FAILING_TITLE(failureCount)}**",
            ["wrap"] = true,
            ["size"] = "Large",
            ["weight"] = "Bolder"
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "TextBlock",
            ["text"] = NotificationVerbiageConstants.HEADLINE_FAILING_DESCRIPTION,
            ["wrap"] = true,
            ["spacing"] = "Small"
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "ActionSet",
            ["actions"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["type"] = "Action.OpenUrl", 
                        ["title"] = NotificationVerbiageConstants.ACTIONS_GOTODASHBOARD,
                        ["url"] = _serverEndpointsOptions.HomePage,
                    }
                },
            ["spacing"] = "Medium"
        });

        return this;
    }
    
    public MicrosoftTeamsRestContentBuilder WithRestoredHeading(int restoreCount)
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "TextBlock",
            ["text"] = $"**{NotificationVerbiageConstants.HEADLINE_RESTORED_TITLE(restoreCount)}**",
            ["wrap"] = true,
            ["size"] = "Large",
            ["weight"] = "Bolder"
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "TextBlock",
            ["text"] = NotificationVerbiageConstants.HEADLINE_RESTORED_DESCRIPTION,
            ["wrap"] = true,
            ["spacing"] = "Small"
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "ActionSet",
            ["actions"] = new JsonArray
            {
                new JsonObject
                {
                    ["type"] = "Action.OpenUrl", 
                    ["title"] = NotificationVerbiageConstants.ACTIONS_GOTODASHBOARD,
                    ["url"] = _serverEndpointsOptions.HomePage,
                }
            },
            ["spacing"] = "Medium"
        });

        return this;
    }

    public MicrosoftTeamsRestContentBuilder WithFailureHealthCheck(GenerateTemplateRequest eventRequest)
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "TextBlock",
            ["text"] = $"**{NotificationVerbiageConstants.HEALTHCHECK_DETAILS_TRIGGEREDON}** {eventRequest.ExecutedOn.ToString(CultureInfo.InvariantCulture)}",
            ["spacing"] = "Small"
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "ColumnSet",
            ["columns"] = new JsonArray()
            {
                new JsonObject
                {
                    ["type"] = "Column",
                    ["width"] = "stretch",
                    ["items"] = new JsonArray()
                    {
                        new JsonObject
                        {
                            ["type"] = "TextBlock",
                            ["text"] = $"**{NotificationVerbiageConstants.HEALTHCHECK_DETAILS_HEALTHSTATUS}** {NotificationVerbiageConstants.HealthStatusChip(eventRequest.JobResult.Status)}",
                            ["wrap"] = true
                        },
                        new JsonObject
                        {
                            ["type"] = "TextBlock",
                            ["text"] = $"**{eventRequest.HealthCheckProfile.Name}**",
                            ["wrap"] = true,
                            ["spacing"] = "Small"
                        },
                        new JsonObject
                        {
                            ["type"] = "TextBlock",
                            ["text"] = eventRequest.JobResult.Description ?? eventRequest.JobResult.Exception?.Message,
                            ["wrap"] = true,
                            ["spacing"] = "Small"
                        }
                    }
                },
                new JsonObject
                {
                    ["type"] = "Column",
                    ["width"] = "auto",
                    ["items"] = new JsonArray()
                    {
                        new JsonObject
                        {
                            ["type"] = "Image",
                            ["url"] = "",
                            ["size"] = "Small"
                        }
                    }
                }
            }
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "ActionSet",
            ["actions"] = new JsonArray
            {
                new JsonObject
                {
                    ["type"] = "Action.OpenUrl",
                    ["title"] = NotificationVerbiageConstants.ACTIONS_VIEWDETAILS,
                    ["url"] = _serverEndpointsOptions.HealthCheckProfile(eventRequest.HealthCheckProfile.Id)
                },
                new JsonObject
                {
                    ["type"] = "Action.OpenUrl",
                    ["title"] = NotificationVerbiageConstants.ACTIONS_ACTIONFAILURE,
                    ["url"] = _serverEndpointsOptions.HealthCheckAuditAction(eventRequest.HealthCheckProfile.Id)
                }
            }
        });
        
        return this;
    }
    
    public MicrosoftTeamsRestContentBuilder WithRestoredHealthCheck(GenerateTemplateRequest eventRequest)
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "TextBlock",
            ["text"] = $"**{NotificationVerbiageConstants.HEALTHCHECK_DETAILS_TRIGGEREDON}** {eventRequest.ExecutedOn.ToString(CultureInfo.InvariantCulture)}",
            ["spacing"] = "Small"
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "ColumnSet",
            ["columns"] = new JsonArray()
            {
                new JsonObject
                {
                    ["type"] = "Column",
                    ["width"] = "stretch",
                    ["items"] = new JsonArray()
                    {
                        new JsonObject
                        {
                            ["type"] = "TextBlock",
                            ["text"] = $"**{NotificationVerbiageConstants.HEALTHCHECK_DETAILS_HEALTHSTATUS}** {NotificationVerbiageConstants.HealthStatusChip(eventRequest.JobResult.Status)}",
                            ["wrap"] = true
                        },
                        new JsonObject
                        {
                            ["type"] = "TextBlock",
                            ["text"] = $"**{eventRequest.HealthCheckProfile.Name}**",
                            ["wrap"] = true,
                            ["spacing"] = "Small"
                        }
                    }
                },
                new JsonObject
                {
                    ["type"] = "Column",
                    ["width"] = "auto",
                    ["items"] = new JsonArray()
                    {
                        new JsonObject
                        {
                            ["type"] = "Image",
                            ["url"] = "",
                            ["size"] = "Small"
                        }
                    }
                }
            }
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "ActionSet",
            ["actions"] = new JsonArray
            {
                new JsonObject
                {
                    ["type"] = "Action.OpenUrl",
                    ["title"] = NotificationVerbiageConstants.ACTIONS_VIEWDETAILS,
                    ["url"] = _serverEndpointsOptions.HealthCheckProfile(eventRequest.HealthCheckProfile.Id)
                }
            }
        });
        
        return this;
    }

    public string Build()
    {
        return new JsonObject
        {
            ["$schema"] = "http://adaptivecards.io/schemas/adaptive-card.json",
            ["type"] = "AdaptiveCard",
            ["version"] = "1.5",
            ["body"] = _rootJson
        }.ToString();
    }
    
}