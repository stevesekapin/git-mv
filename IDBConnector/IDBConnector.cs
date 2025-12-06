namespace IDBConnector
{
    public interface IDBConnector
    {
        Task<bool> PingAsync();
        Task InsertManyAsync(IEnumerable<string> data);
        Task<string?> GetOneAsync(int index);
        Task<bool> ConnectAsync(string connectionString);
    }
}
