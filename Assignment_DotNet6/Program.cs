using Assignment_DotNet6;
using Assignment_DotNet6.Core;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigureLogs();
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.ConfigureServices(builder.Configuration, builder.Environment);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRequestResponseLogger();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
SeedDB.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider).Wait();
app.UseCors("ClientPermission");
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();


#region Logs
void ConfigureLogs()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", reloadOnChange: true, optional: true)
        .Build();


    Log.Logger = new LoggerConfiguration()
         .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureELS(config))
        .WriteTo.File($"./failures.log", rollingInterval: RollingInterval.Day)
        .CreateLogger();



}

ElasticsearchSinkOptions ConfigureELS(IConfiguration configuration)
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var oibj = new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearchConfig:URL"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name}_DotNetSixApplication_{env}_{DateTime.Now}",
    };
    

    return oibj;
}

#endregion