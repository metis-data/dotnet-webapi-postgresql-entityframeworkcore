using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace dotnet_webapi_postgresql_entityframeworkcore.IntegrationTests;

public class TestContainersFactory : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        if(Environment.GetEnvironmentVariable("MOCK_CONTAINER_DEPENDENCIES") != null){
            var databaseContainer = new ContainerBuilder()
                .WithImage("public.ecr.aws/o2c0x5x8/metis-demo-mini-db:latest")
                .WithPortBinding(5432, true)
                .WithNetworkAliases("database")
                .Build();
            await databaseContainer.StartAsync();

            Environment.SetEnvironmentVariable("DATABASE_CONNECTION", $"Server=127.0.0.1;Port={databaseContainer.GetMappedPublicPort(5432)};Database=demo;Username=postgres;Password=postgres");

            var otelContainer = new ContainerBuilder()
                .WithImage("public.ecr.aws/o2c0x5x8/metis-otel-collector:latest")
                .WithPortBinding(4318, true)
                .WithEnvironment("METIS_API_KEY", Environment.GetEnvironmentVariable("METIS_API_KEY"))
                .WithEnvironment("CONNECTION_STRING", "postgresql://postgres:postgres@database:5432/demo?schema=imdb")
                .WithEnvironment("LOG_LEVEL", "debug")
                .Build();
            await otelContainer.StartAsync();
        }
    }

    public Task DisposeAsync()
    {
        return Task.Delay(TimeSpan.FromSeconds(30));
    }
}

[CollectionDefinition("Context collection")]
public class InMemoryDbContextFixtureCollection : ICollectionFixture<TestContainersFactory>, 
    ICollectionFixture<CustomWebApplicationFactory<Program>>
{
}

[Collection("Context collection")]
public class TitleRatingsTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TitleRatingsTests(CustomWebApplicationFactory<Program> factory, TestContainersFactory testContainersFactory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/titles/ratings/best")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
    }
}