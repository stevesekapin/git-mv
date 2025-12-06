using IDBConnector;
using MongoDbConnector;
using PostgresConnector;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("====================================");
        Console.WriteLine(" Database Connector REPL Console ");
        Console.WriteLine("====================================");

        while (true)
        {
            Console.WriteLine("\nSelect a Database:");
            Console.WriteLine("1. MongoDB");
            Console.WriteLine("2. PostgreSQL");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");

            var input = Console.ReadLine();

            if (input == "0")
                break;

            IDBConnector.IDBConnector? connector = input switch
            {
                "1" => new MongoDbConnector.MongoDbConnector(),
                "2" => new PostgresConnector.PostgresDbConnector(),
                _ => null
            };

            if (connector == null)
            {
                Console.WriteLine("Invalid choice. Try again.");
                continue;
            }

            Console.Write("\nEnter connection string: ");
            string? connectionString = Console.ReadLine();

            if (!await connector.ConnectAsync(connectionString!))
            {
                Console.WriteLine("❌ Failed to connect to database.");
                continue;
            }

            Console.WriteLine("✔ Connected successfully!");
            Console.WriteLine("Pinging DB...");

            var pingSuccess = await connector.PingAsync();
            Console.WriteLine(pingSuccess ? "✔ Ping OK!" : "❌ Ping failed");

            Console.WriteLine("\nInserting 20 sample records...");
            var data = Enumerable.Range(1, 20).Select(i => $"Record #{i}");
            await connector.InsertManyAsync(data);

            Console.WriteLine("Retrieving item at index 5...");
            var result = await connector.GetOneAsync(5);
            Console.WriteLine($"Result: {result}");

            Console.WriteLine("\nOperation Complete!");
        }

        Console.WriteLine("\nExiting the app... 👋");
    }
}
