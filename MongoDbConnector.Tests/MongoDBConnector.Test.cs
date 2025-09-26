using System.Threading.Tasks;
using FluentAssertions;
using Testcontainers.MongoDb;
using Xunit;
using MongoDbConnector;

public class MongoDBConnectorTests : IAsyncLifetime
{
    // Spins up a real MongoDB in Docker for the tests
    private readonly MongoDbContainer _mongo = new MongoDbBuilder()
        .WithImage("mongo:7")
        .Build();

    public Task InitializeAsync() => _mongo.StartAsync();
    public Task DisposeAsync()    => _mongo.DisposeAsync().AsTask();

    [Fact]
    public void Ping_ReturnsTrue_WhenContainerIsRunning()
    {
        var connector = new MongoDBConnector(_mongo.GetConnectionString());
        connector.Ping().Should().BeTrue();
    }

    [Fact]
    public void Ping_ReturnsFalse_WithInvalidConnection()
    {
        var bad = new MongoDBConnector("mongodb://127.0.0.1:1");
        bad.Ping().Should().BeFalse();
    }
}
