using IDBConnector;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbConnector
{
    public class MongoDbConnector : IDBConnector.IDBConnector
    {
        private IMongoDatabase? _database;
        private IMongoCollection<BsonDocument>? _collection;

        public async Task<bool> ConnectAsync(string connectionString)
        {
            try
            {
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase("TestDb"); // You can rename if needed
                _collection = _database.GetCollection<BsonDocument>("SampleData");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PingAsync()
        {
            if (_database == null)
                return false;

            try
            {
                await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task InsertManyAsync(IEnumerable<string> data)
        {
            if (_collection == null)
                throw new InvalidOperationException("Not connected to database!");

            var docs = data.Select(x => new BsonDocument("Value", x));
            await _collection.InsertManyAsync(docs);
        }

        public async Task<string?> GetOneAsync(int index)
        {
            if (_collection == null)
                throw new InvalidOperationException("Not connected to database!");

            var results = await _collection.Find(new BsonDocument()).ToListAsync();

            if (index < 0 || index >= results.Count)
                return null;

            return results[index]["Value"].AsString;
        }
    }
}
