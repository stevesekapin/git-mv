using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbConnector;

public class MongoDBConnector
{
    private readonly string _connectionString;
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _adminDb;

    public MongoDBConnector(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _client = new MongoClient(_connectionString);
        _adminDb = _client.GetDatabase("admin");
    }

    // Pings MongoDB. Returns true on success, false on failure. No parameters.
    public bool Ping()
    {
        try
        {
            _adminDb.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            return true;
        }
        catch
        {
            return false;
        }
    }
}
