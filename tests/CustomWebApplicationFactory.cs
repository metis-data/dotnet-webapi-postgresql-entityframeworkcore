using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using dotnet_webapi_postgresql_entityframeworkcore.Models;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;

namespace dotnet_webapi_postgresql_entityframeworkcore;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

        builder
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                if(connectionString == null){
                    var configuration = config.Build();
                    connectionString = configuration.GetConnectionString("DefaultConnection");
                }
            })
            .ConfigureServices(services =>
            {
                services.AddOpenTelemetry().WithTracing(b =>
                {
                    b
                    .AddSource("entityframeworkcore")
                    .ConfigureResource(resource =>
                        resource.AddService(
                        serviceName: "entityframeworkcore",
                        serviceVersion: "1"))
                    .AddAspNetCoreInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql()
                    .AddConsoleExporter()
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri("http://127.0.0.1:4318/v1/traces");
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                    });
                });

                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ImdbContext>));
                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                services.Remove(dbConnectionDescriptor);

                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new NpgsqlConnection(connectionString);
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<ImdbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseNpgsql(connection);
                });
            });
    }
}