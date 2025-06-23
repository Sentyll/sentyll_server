using Sentyll.Infrastructure.Server.Startup;
using Sentyll.UI.Middleware;
using Sentyll.UI.Startup.Documentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.RegisterOpenApiDocumentation();

var store = builder.AddSentyllServer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/");
    app.UseHsts();
}
else
{
    app.UseOpenApiDocumentation();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSentyllServer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}",
    defaults: new { controller = "Home", action = "Index" });

app.Run();