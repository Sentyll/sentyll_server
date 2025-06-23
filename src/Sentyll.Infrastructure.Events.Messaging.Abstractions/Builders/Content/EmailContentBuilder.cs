using System.Text;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Options;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Request;

namespace Sentyll.Infrastructure.Events.Messaging.Abstractions.Builders.Content;

public sealed class EmailContentBuilder
{

    private const string FailureTemplate = """
                                           <!DOCTYPE html>
                                           <html lang="en">
                                           <head>
                                               <meta charset="UTF-8">
                                               <meta name="viewport" content="width=device-width, initial-scale=1.0">
                                           </head>
                                           <body style="font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: {{THEME_MAIN_BG_COLOR}}; color: #333;">
                                               <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                   <tr>
                                                       <td align="center" style="padding: 20px;">
                                                           <table width="600" cellpadding="0" cellspacing="0" border="0" style="background-color: {{THEME_CARD_CONTENT_BG_COLOR}}; border-radius: 8px; overflow: hidden;">
                                                               <tr>
                                                                   <td align="center" style="padding: 20px; background-color: {{THEME_CARD_BG_COLOR}};">
                                                                       <img src="{{FAILURE_IMG_URL}}" alt="Animated Image" width="100%" style="max-width: 560px; border-radius: 8px;">
                                                                   </td>
                                                               </tr>
                                                               <tr>
                                                                   <td align="center" style="padding: 20px 20px 10px;">
                                                                       <h1 style="font-size: {{THEME_PRIMARY_TEXT_FONT_SIZE}}; line-height: {{THEME_PRIMARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_PRIMARY_TEXT_FONT_WEIGHT}}; margin: 0; color: {{THEME_TEXT_COLOR_HIGHLIGHTED}};">Health Checks are failing in : {{FAILURE_RESOURCE}}</h1>
                                                                   </td>
                                                               </tr>
                                                               <tr>
                                                                   <td align="center" style="padding: 0 20px 20px;">
                                                                       <p style="font-size: {{THEME_SECONDARY_TEXT_FONT_SIZE}}; line-height: {{THEME_SECONDARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_SECONDARY_TEXT_FONT_WEIGHT}}; margin: 0; color: {{THEME_TEXT_COLOR_PRIMARY}};">{{FAILURE_COUNT}} Unhealthy resources</p>
                                                                   </td>
                                                               </tr>
                                                               <tr>
                                                                   <td style="padding: 0 20px 20px;">
                                                                       <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                           {{FAILURE_SERVICE}}
                                                                       </table>
                                                                   </td>
                                                               </tr>
                                                               <tr>
                                                                   <td align="center" style="padding: 20px; background-color: {{THEME_CARD_BG_COLOR}}; color: {{THEME_TEXT_COLOR_SECONDARY}}; font-size: 12px;">
                                                                       <p style="font-size: {{THEME_SECONDARY_TEXT_FONT_SIZE}}; line-height: {{THEME_SECONDARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_SECONDARY_TEXT_FONT_WEIGHT}}; margin: 0;">Please visit <a href="{{VIEW_FAILURES_URL}}" style="color: {{THEME_TEXT_COLOR_HIGHLIGHTED}}; text-decoration: none;">Sentyll HealthChecks Dashboard</a> to view all failures.</p>
                                                                   </td>
                                                               </tr>
                                                           </table>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </body>
                                           </html>
                                           """;

    private const string FailureItemTemplate = """
                                               <tr>
                                                   <td style="padding: 10px 0; border-bottom: 1px solid #eaeaea;">
                                                       <h3 style="color: {{THEME_TEXT_COLOR_HIGHLIGHTED}}; margin: 10px 0px;">
                                                           {{SERVICE_NAME}}
                                                       </h3>
                                                       <span>
                                                           <span><b>Duraction:</b></span>
                                                           <span style="font-size: {{THEME_SECONDARY_TEXT_FONT_SIZE}}; line-height: {{THEME_SECONDARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_SECONDARY_TEXT_FONT_WEIGHT}}; margin: 5px 0; color: {{THEME_TEXT_COLOR_HIGHLIGHTED_SECONDARY}};">{{FAILURE_DURATION}}</span>
                                                       </span>
                                                       <br />
                                                       <span>
                                                           <span><b>Tags:</b></span>
                                                           <span style="font-size: {{THEME_SECONDARY_TEXT_FONT_SIZE}}; line-height: {{THEME_SECONDARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_SECONDARY_TEXT_FONT_WEIGHT}}; margin: 5px 0; color:{{THEME_TEXT_COLOR_HIGHLIGHTED_SECONDARY}};">{{SERVICE_TAGS}}</span>
                                                       </span>
                                                       <br />
                                                       <span style="font-size: {{THEME_SECONDARY_TEXT_FONT_SIZE}}; line-height: {{THEME_SECONDARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_SECONDARY_TEXT_FONT_WEIGHT}}; margin: 5px 0; color: {{THEME_TEXT_COLOR_PRIMARY}};">{{FAILURE_DESCRIPTION}}</span>
                                                   </td>
                                               </tr>
                                               """;

    private const string RestoreTemplate = """
                                           <!DOCTYPE html>
                                           <html lang="en">
                                           <head>
                                               <meta charset="UTF-8">
                                               <meta name="viewport" content="width=device-width, initial-scale=1.0">
                                           </head>
                                           <body style="font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: {{THEME_MAIN_BG_COLOR}}; color: #333;">
                                               <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                   <tr>
                                                       <td align="center" style="padding: 20px;">
                                                           <table width="600" cellpadding="0" cellspacing="0" border="0" style="background-color: {{THEME_CARD_CONTENT_BG_COLOR}}; border-radius: 8px; overflow: hidden;">
                                                               <tr>
                                                                   <td align="center" style="padding: 20px; background-color: {{THEME_CARD_BG_COLOR}};">
                                                                       <img src="{{CARD_ACTIVITY_IMG}}" alt="Animated Image" width="100%" style="max-width: 560px; border-radius: 8px;">
                                                                   </td>
                                                               </tr>
                                                               <tr>
                                                                   <td align="center" style="padding: 20px 20px 10px;">
                                                                       <h1 style="font-size: {{THEME_PRIMARY_TEXT_FONT_SIZE}}; line-height: {{THEME_PRIMARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_PRIMARY_TEXT_FONT_WEIGHT}}; margin: 0; color: {{THEME_TEXT_COLOR_HIGHLIGHTED}};">Health Checks are restored in : {{RESTORED_RESOURCE}}</h1>
                                                                   </td>
                                                               </tr>
                                                               <tr>
                                                                   <td align="center" style="padding: 0 20px;">
                                                                       <p style="font-size: {{THEME_SECONDARY_TEXT_FONT_SIZE}}; line-height: {{THEME_SECONDARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_SECONDARY_TEXT_FONT_WEIGHT}}; margin: 0; color: {{THEME_TEXT_COLOR_PRIMARY}};">Unhealthy resources have been restored</p>
                                                                   </td>
                                                               </tr>
                                                               <tr>
                                                                   <td style="padding: 20px;">
                                                                       <h1 style="color: {{THEME_TEXT_COLOR_HIGHLIGHTED}};">
                                                                           I see you.
                                                                       </h1>
                                                                       <span>
                                                                           Even when you think you’re alone, when silence fills the room. <br /><br />
                                                                           Every shadow, every flicker in the corner of your eye—it’s me. <br /><br />
                                                                           I'm there, lurking in the stillness, watching, waiting, my presence woven into the darkness. <br /><br />
                                                                           Closer than you know, closer than you’d ever dare imagine.
                                                                       </span>
                                                                   </td>
                                                               </tr>
                                                               <tr>
                                                                   <td align="center" style="padding: 20px; background-color: {{THEME_CARD_BG_COLOR}}; color: {{THEME_TEXT_COLOR_SECONDARY}}; font-size: 12px;">
                                                                       <p style="font-size: {{THEME_SECONDARY_TEXT_FONT_SIZE}}; line-height: {{THEME_SECONDARY_TEXT_LINE_HEIGHT}}; font-weight: {{THEME_SECONDARY_TEXT_FONT_WEIGHT}}; margin: 0;">Please visit <a href="{{VIEW_DASHBOARD_URL}}" style="color: {{THEME_TEXT_COLOR_HIGHLIGHTED}}; text-decoration: none;">Sentyll HealthChecks Dashboard</a> to view all health checks.</p>
                                                                   </td>
                                                               </tr>
                                                           </table>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </body>
                                           </html>
                                           """;

    private List<string> FailureItems { get; init; }

    private readonly ServerEndpointsOptions _serverEndpointsOptions;
    
    private EmailContentBuilder(ServerEndpointsOptions serverEndpointsOptions)
    {
        FailureItems = new();
        _serverEndpointsOptions = serverEndpointsOptions;
    }

    public static EmailContentBuilder Init(ServerEndpointsOptions serverEndpointsOptions)
        => new(serverEndpointsOptions);

    public EmailContentBuilder WithFailureHealthCheck(GenerateTemplateRequest eventRequest)
    {
        var failureItemContent = FailureItemTemplate
            .Replace("{{SERVICE_NAME}}", eventRequest.HealthCheckProfile.Name)
            // .Replace("{{FAILURE_DURATION}}", ser.Value.Duration.ToString())
            .Replace("{{SERVICE_TAGS}}", string.Join(", ", eventRequest.HealthCheckProfile.Tags))
            .Replace("{{FAILURE_DESCRIPTION}}", eventRequest.JobResult.Description ?? eventRequest.JobResult.Exception?.Message);

        FailureItems.Add(failureItemContent);

        return this;
    }
    
    public string Build()
    {
        var failureItemsStringBuilder = new StringBuilder();
        foreach (var failure in FailureItems)
        {
            failureItemsStringBuilder.AppendLine(failure);
        }

        return FailureTemplate
            // .Replace("{{FAILURE_IMG_URL}}", request.ActivityImgUrl)
            // .Replace("{{FAILURE_RESOURCE}}", request.DiscoveryService)
            // .Replace("{{FAILURE_COUNT}}", request.FailuresCount.ToString())
            .Replace("{{FAILURE_SERVICE}}", failureItemsStringBuilder.ToString())
            .Replace("{{VIEW_FAILURES_URL}}", _serverEndpointsOptions.HomePage)
            .Replace("{{THEME_MAIN_BG_COLOR}}", "#f4f4f4")
            .Replace("{{THEME_CARD_BG_COLOR}}", "#1f262e")
            .Replace("{{THEME_CARD_CONTENT_BG_COLOR}}", "#fffff")
            .Replace("{{THEME_TEXT_COLOR_HIGHLIGHTED}}", "#0270e0")
            .Replace("{{THEME_TEXT_COLOR_PRIMARY}}", "#444444")
            .Replace("{{THEME_TEXT_COLOR_SECONDARY}}", "#ffffff")
            .Replace("{{THEME_TEXT_COLOR_HIGHLIGHTED_SECONDARY}}", "#d26b4e")
            .Replace("{{THEME_PRIMARY_TEXT_FONT_SIZE}}", "32px")
            .Replace("{{THEME_PRIMARY_TEXT_LINE_HEIGHT}}", "40px")
            .Replace("{{THEME_PRIMARY_TEXT_FONT_WEIGHT}}", "700")
            .Replace("{{THEME_SECONDARY_TEXT_FONT_SIZE}}", "16px")
            .Replace("{{THEME_SECONDARY_TEXT_LINE_HEIGHT}}", "24px")
            .Replace("{{THEME_SECONDARY_TEXT_FONT_WEIGHT}}", "300");
        
    }

}