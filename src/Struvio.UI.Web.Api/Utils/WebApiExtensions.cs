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

    extension(IApplicationBuilder app)
    {
        public IApplicationBuilder UseAccsivoAuthentication(Func<HttpContext, Task> notAuthenticated, Func<HttpContext, Task> denyAccess, Func<HttpContext, Task> expiredSession)
        {
            return app.Use(async (httpContext, next) =>
            {
                // Servisleri çekiyoruz
                IPermissionService permissionService = httpContext.RequestServices.GetRequiredService<IPermissionService>();
                IAccountService accountService = httpContext.RequestServices.GetRequiredService<IAccountService>();
                IGeneralSettings generalSettings = httpContext.RequestServices.GetRequiredService<IGeneralSettings>();

                RolePermissionLineModel[] permissions = await permissionService.GetAllRolePermissionLinesAsync();
                string path = httpContext.Request.Path.ToString();

                // RouteValue'dan controller ve action isimlerini al
                httpContext.Request.RouteValues.TryGetValue("controller", out var controllerObj);
                httpContext.Request.RouteValues.TryGetValue("action", out var actionObj);

                string? controller = controllerObj?.ToString();
                string? action = actionObj?.ToString();

                bool isMvc = !string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action);


                // İlgili permission var mı?
                bool permissionExists = isMvc
                    ? permissions.Any(x => x.ControllerName == controller && x.ActionName == action)
                    : permissions.Any(x => x.Path == path);

                // Eğer ilgili permission yoksa, herkes erişebilir
                if (!permissionExists)
                {
                    await next();
                    return;
                }

                // Kullanıcı kimlik doğrulaması yapılmış mı?
                if (httpContext.User?.Identity?.IsAuthenticated != true)
                {
                    await notAuthenticated(httpContext);
                    return;
                }




                // Kullanıcı id'sini çek
                Claim? nameIdentifierClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (nameIdentifierClaim == null || !Guid.TryParse(nameIdentifierClaim.Value, out Guid userId) || userId == Guid.Empty)
                {
                    await notAuthenticated(httpContext);
                    return;
                }

                // Kullanıcının geçerli bir oturumu var mı
                if (!await accountService.ValidateOrCloseExpiredSessionAsync(userId))
                {
                    await expiredSession(httpContext);
                    return;
                }

                // Kullanıcının rolleri
                Guid[]? roleIds = await accountService.GetRoleIdsInDefaultOrganizationAsync(userId);

                if (roleIds == null || roleIds.Length == 0)
                {
                    await denyAccess(httpContext);
                    return;
                }

                // Kullanıcının rolü ile permission eşleşiyor mu?
                bool isAuthorized = isMvc
                    ? permissions.Any(x => x.ControllerName == controller && x.ActionName == action && roleIds.Contains(x.RoleId))
                    : permissions.Any(x => x.Path == path && roleIds.Contains(x.RoleId));

                if (!isAuthorized)
                {
                    await denyAccess(httpContext);
                    return;
                }

                // Her şey doğruysa devam et

                // oturum süresini yenile   

                await next();
            });
        }
    }
}
