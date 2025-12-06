using IDBConnector;
using Npgsql;

namespace PostgresConnector
{
    public class PostgresDbConnector : IDBConnector.IDBConnector
    {
        private NpgsqlConnection? _connection;

        public async Task<bool> ConnectAsync(string connectionString)
        {
            try
            {
                _connection = new NpgsqlConnection(connectionString);
                await _connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PingAsync()
        {
            if (_connection == null)
                return false;

            try
            {
                using var cmd = new NpgsqlCommand("SELECT 1", _connection);
                await cmd.ExecuteScalarAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task InsertManyAsync(IEnumerable<string> data)
        {
            if (_connection == null)
                throw new InvalidOperationException("Not connected to database!");

            string createTable = @"
                CREATE TABLE IF NOT EXISTS sample_data (
                    id SERIAL PRIMARY KEY,
                    value TEXT NOT NULL
                );";
            using var cmd = new NpgsqlCommand(createTable, _connection);
            await cmd.ExecuteNonQueryAsync();

            foreach (var item in data)
            {
                using var insertCmd = new NpgsqlCommand(
                    "INSERT INTO sample_data (value) VALUES (@val)",
                    _connection
                );
                insertCmd.Parameters.AddWithValue("@val", item);
                await insertCmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<string?> GetOneAsync(int index)
        {
            if (_connection == null)
                throw new InvalidOperationException("Not connected to database!");

            using var cmd = new NpgsqlCommand(
                "SELECT value FROM sample_data ORDER BY id LIMIT 1 OFFSET @idx",
                _connection
            );
            cmd.Parameters.AddWithValue("@idx", index);

            return (string?)await cmd.ExecuteScalarAsync();
        }
    }
}
