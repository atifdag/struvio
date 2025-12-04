using Microsoft.Extensions.Options;
using Struvio.Domain.Entities;
using Struvio.Persistence;

namespace Struvio.UI.Web.Api.Utils;

public static class WebApiExtensions
{
    extension(IApplicationBuilder app)
    {
        public void RemoveAspHeaders()
        {
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Remove("Server");
                    context.Response.Headers.Remove("X-Powered-By");
                    return Task.CompletedTask;
                });

                await next();
            });
        }

        public IApplicationBuilder UseLocalization()
        {
            RequestLocalizationOptions options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>()!.Value;
            return app.UseRequestLocalization(options);
        }


        public IApplicationBuilder UseInstallation()
        {
            IServiceProvider serviceProvider = app.ApplicationServices.GetService<IServiceProvider>()!;
            using IServiceScope scope = serviceProvider.CreateScope();
            return app.UseMiddleware<InstallationMiddleware>();
        }

    }



    extension(HttpContext httpContext)
    {
        public string? GetIpAddress()
        {
            if (httpContext == null) return null;

            // Reverse proxy'ler üzerinden gelen IP
            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor) &&
                !string.IsNullOrWhiteSpace(forwardedFor))
            {
                // Virgülle ayrılmışsa ilk IP'yi al (ilk proxy kaynağı)
                return forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim();
            }

            // Doğrudan bağlantı IP'si
            return httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }

        public string? GetUserAgent()
        {
            return httpContext?.Request?.Headers.UserAgent.ToString();
        }
    }

    extension(WebApplicationBuilder webApplicationBuilder)
    {
        public IdentityBuilder BuildIdentity()
        {
            return webApplicationBuilder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = false;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                }).AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
