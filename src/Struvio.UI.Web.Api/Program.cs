var builder = WebApplication.CreateBuilder(args);

// Serilog yapılandırması - appsettings.json'dan okunur
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
var apiVersion = assemblyVersion != null ? $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}.{assemblyVersion.Revision}" : "0.0.0";

string apiName = "Struvio.UI.Web.Api";


builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IStruvioLogger, StruvioLogger>();

// Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<Struvio.Persistence.ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Identity
builder.BuildIdentity();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
// Cache Services
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Struvio:";
});
builder.Services.AddSingleton<ICacheSettings>(sp =>
{
    var cacheSettings = new Struvio.Application.Caching.CacheSettings();
    builder.Configuration.GetSection("Cache").Bind(cacheSettings);
    return cacheSettings;
});
builder.Services.AddScoped<ICacheService, Struvio.Application.Caching.CacheService>();

builder.Services.AddScoped<ICurrentUserContext, Struvio.Application.CurrentUserContext>();
builder.Services.AddScoped<IPrincipal>(provider => provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User
    ?? throw new InvalidOperationException("User context is not available."));

// JWT Authentication yapılandırması
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey configuration is missing");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer configuration is missing");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience configuration is missing");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ClockSkew = TimeSpan.Zero // Token expiration'ı tam olarak kontrol et
    };
});

builder.Services.AddAuthorization();

// Health Checks - Merkezileştirilmiş yapılandırma
builder.Services.AddStruvioHealthChecks();

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_1;

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = apiName;
        document.Info.Version = apiVersion;
        document.Info.Description = $"{apiName} - Version {apiVersion}";
        return Task.CompletedTask;
    });
});

builder.Services.AddWebEncoders(o =>
{
    o.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});


var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.RemoveAspHeaders();

app.UseLocalization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

// TODO: IPermissionService ve IAccountService implement edildikten sonra aktif edilecek
// app.UseAccsivoAuthentication(async httpContext =>
// {
//     Log.Warning("Kimlik oturumu gerektiren eri�im! Path: {Path}", httpContext.Request.Path);
//     httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//     await httpContext.Response.WriteAsJsonAsync(new { success = false, message = LanguageTexts.SessionRequired });
// },
// async httpContext =>
// {
//     Log.Warning("Yetkisiz eri�im! Path: {Path}", httpContext.Request.Path);
//     httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
//     await httpContext.Response.WriteAsJsonAsync(new { success = false, message = LanguageTexts.AccessDenied });
// },
// async httpContext =>
// {
//     Log.Warning("Oturum s�resi dolmu�. Path: {Path}", httpContext.Request.Path);
//     httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//     await httpContext.Response.WriteAsJsonAsync(new { success = false, message = LanguageTexts.SessionExpired });
// });

// Health Check Endpoints - Merkezileştirilmiş yapılandırma
app.MapStruvioHealthChecks();

app.MapControllers();

try
{
    Log.Information("Struvio.UI.Web.Api başlatılıyor...");
    app.Run();
    Log.Information("Struvio.UI.Web.Api durduruldu.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Struvio.UI.Web.Api başlatılamadı!");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
