using System.Globalization;
using System.Text.Json.Nodes;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Constants;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Options;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Request;

namespace Sentyll.Infrastructure.Events.Messaging.Slack.Builders;

internal sealed class SlackRestContentBuilder
{

    private readonly JsonArray _rootJson;
    
    private readonly ServerEndpointsOptions _serverEndpointsOptions;
    
    private SlackRestContentBuilder(ServerEndpointsOptions serverEndpointsOptions)
    {
        _serverEndpointsOptions = serverEndpointsOptions;
        _rootJson = new();
    }

    public static SlackRestContentBuilder Init(ServerEndpointsOptions serverEndpointsOptions)
        => new(serverEndpointsOptions);
    
    public SlackRestContentBuilder WithDivider()
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "divider" 
        });

        return this;
    }
    
    public SlackRestContentBuilder WithFailureHeading(int failureCount)
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "section",
            ["text"] = new JsonObject
            {
                ["type"] = "mrkdwn",
                ["text"] = $"{NotificationVerbiageConstants.HEADLINE_FAILING_TITLE(failureCount)} \n\n {NotificationVerbiageConstants.HEADLINE_FAILING_DESCRIPTION}"
            },
            ["accessory"] = new JsonObject
            {
                ["type"] = "button",
                ["text"] = new JsonObject
                {
                    ["type"] = "plain_text",
                    ["text"] = NotificationVerbiageConstants.ACTIONS_GOTODASHBOARD,
                    ["emoji"] = false
                },
                ["url"] = _serverEndpointsOptions.HomePage
            }
        });

        return this;
    }
    
    public SlackRestContentBuilder WithRestoredHeading(int restoredCount)
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "section",
            ["text"] = new JsonObject
            {
                ["type"] = "mrkdwn",
                ["text"] = $"{NotificationVerbiageConstants.HEADLINE_RESTORED_TITLE(restoredCount)} \n\n {NotificationVerbiageConstants.HEADLINE_RESTORED_DESCRIPTION}"
            },
            ["accessory"] = new JsonObject
            {
                ["type"] = "button",
                ["text"] = new JsonObject
                {
                    ["type"] = "plain_text",
                    ["text"] = NotificationVerbiageConstants.ACTIONS_GOTODASHBOARD,
                    ["emoji"] = false
                },
                ["url"] = _serverEndpointsOptions.HomePage
            }
        });

        return this;
    }
    
    public SlackRestContentBuilder WithFailureHealthCheck(GenerateTemplateRequest eventRequest)
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "context",
            ["elements"] = new JsonArray
            {
                new JsonObject
                {
                    ["type"] = "mrkdwn", 
                    ["text"] = NotificationVerbiageConstants.HEALTHCHECK_DETAILS_TRIGGEREDON
                },
                new JsonObject
                {
                    ["type"] = "mrkdwn", 
                    ["text"] = $"*{eventRequest.ExecutedOn.ToString(CultureInfo.InvariantCulture)}*"
                }
            }
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "section",
            ["text"] = new JsonObject
            {
                ["type"] = "mrkdwn",
                ["text"] = string.Format("*{0}*: {1}\n\n*{2}*\n\n{3}",
                        NotificationVerbiageConstants.HEALTHCHECK_DETAILS_OVERVIEW,
                        NotificationVerbiageConstants.HealthStatusChip(eventRequest.JobResult.Status), 
                        eventRequest.HealthCheckProfile.Name, 
                        eventRequest.JobResult.Description ?? eventRequest.JobResult.Exception?.Message
                    )
            },
            ["accessory"] = new JsonObject
            {
                ["type"] = "image",
                ["image_url"] = "https://www.shutterstock.com/image-vector/realistic-bomb-burning-fuse-emitting-600nw-2474308221.jpg",
                ["alt_text"] = "Azure Function App"
            }
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "actions",
            ["elements"] = new JsonArray
            {
                new JsonObject
                {
                    ["type"] = "button",
                    ["style"] = "danger",
                    ["text"] = new JsonObject
                    {
                        ["type"] = "plain_text",
                        ["text"] = NotificationVerbiageConstants.ACTIONS_VIEWDETAILS,
                        ["emoji"] = true
                    },
                    ["url"] = _serverEndpointsOptions.HealthCheckProfile(eventRequest.HealthCheckProfile.Id)
                },
                new JsonObject
                {
                    ["type"] = "button",
                    ["style"] = "primary",
                    ["text"] = new JsonObject
                    {
                        ["type"] = "plain_text",
                        ["text"] = NotificationVerbiageConstants.ACTIONS_ACTIONFAILURE,
                        ["emoji"] = true
                    },
                    ["url"] = _serverEndpointsOptions.HealthCheckAuditAction(eventRequest.HealthCheckProfile.Id)
                }
            }
        });
        
        return this;
    }
    
    public SlackRestContentBuilder WithRestoredHealthCheck(GenerateTemplateRequest eventRequest)
    {
        _rootJson.Add(new JsonObject
        {
            ["type"] = "context",
            ["elements"] = new JsonArray
            {
                new JsonObject
                {
                    ["type"] = "mrkdwn", 
                    ["text"] = NotificationVerbiageConstants.HEALTHCHECK_DETAILS_TRIGGEREDON
                },
                new JsonObject
                {
                    ["type"] = "mrkdwn", 
                    ["text"] = $"*{eventRequest.ExecutedOn.ToString(CultureInfo.InvariantCulture)}*"
                }
            }
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "section",
            ["text"] = new JsonObject
            {
                ["type"] = "mrkdwn",
                ["text"] = string.Format("*{0}*: {1}\n\n*{2}*",
                        NotificationVerbiageConstants.HEALTHCHECK_DETAILS_OVERVIEW,
                        NotificationVerbiageConstants.HealthStatusChip(eventRequest.JobResult.Status), 
                        eventRequest.HealthCheckProfile.Name
                    )
            },
            ["accessory"] = new JsonObject
            {
                ["type"] = "image",
                
                //TODO: Inject a service to provide these URLS.
                ["image_url"] = "https://www.shutterstock.com/image-vector/realistic-bomb-burning-fuse-emitting-600nw-2474308221.jpg",
                ["alt_text"] = "Azure Function App"
            }
        });
        
        _rootJson.Add(new JsonObject
        {
            ["type"] = "actions",
            ["elements"] = new JsonArray
            {
                new JsonObject
                {
                    ["type"] = "button",
                    ["style"] = "danger",
                    ["text"] = new JsonObject
                    {
                        ["type"] = "plain_text",
                        ["text"] = NotificationVerbiageConstants.ACTIONS_VIEWDETAILS,
                        ["emoji"] = true
                    },
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
            ["blocks"] = _rootJson
        }.ToString();
    }
    
}
