var builder = WebApplication.CreateBuilder(args);


var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
var apiVersion = assemblyVersion != null ? $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}.{assemblyVersion.Revision}" : "0.0.0";

string apiName = "Struvio.UI.Web.Api";


builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IStruvioLogger, StruvioLogger>();

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

}

app.UseAuthorization();

app.MapControllers();

app.Run();
