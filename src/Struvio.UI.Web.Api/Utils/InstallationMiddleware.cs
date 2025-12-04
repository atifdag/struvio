using Struvio.Persistence;
using Struvio.Setup;

namespace Struvio.UI.Web.Api.Utils;

public class InstallationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        ApplicationDbContext? dbContext = httpContext.RequestServices.GetService<ApplicationDbContext>() ?? throw new Exception("DbContext veritabanı bulunamadı!");
        if (Installer.IsInstalled(dbContext))
        {
            await _next(httpContext);
        }
    }
}
