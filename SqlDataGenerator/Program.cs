using Polly;
using SqlDataGenerator;
using SqlDataGenerator.Library;

int batchSize = 10000;
int totalBooks = 1000000;

var dataGenerator = new DataGenerator();
using var repository = CreateRepository(args);
repository.OpenConnection();

var policy = Policy.Handle<Exception>().RetryForever(Console.WriteLine);

for (var i = repository.GetMaxId() + 1; i < totalBooks; i += batchSize)
{
    var batch = dataGenerator.GenerateBatch(i, batchSize);
    Console.WriteLine($"Generated batch #{i}");
    policy.Execute(() => repository.InsertBatch(batch));
    Console.WriteLine($"Inserted batch #{i}");
}

IBookRepository CreateRepository(string[] args)
{
    bool isCitus = args.Any() && args[0] == "citus";

    if (isCitus)
    {
        return new PostgresBookRepository("Server=localhost;Port=25432;Uid=postgres;");
    }
    
    int port = args.Any() && int.TryParse(args[0], out int parsed)
        ? parsed
        : 5432;

    return new PostgresBookRepository($"Server=localhost;Port={port};Database=books;Uid=postgres;Pwd=postgres;");
}