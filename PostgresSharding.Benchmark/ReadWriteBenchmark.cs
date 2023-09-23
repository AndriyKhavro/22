using BenchmarkDotNet.Attributes;
using SqlDataGenerator.Library;

namespace PostgresSharding.Benchmark;

public class ReadWriteBenchmark
{
    [Params(nameof(ConnectionStrings.Fdw), nameof(ConnectionStrings.NoSharding), nameof(ConnectionStrings.Citus))]
    public string Connection = null!;

    private PostgresBookRepository _bookRepository = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var connectionString = ConnectionStrings.All[Connection];
        _bookRepository = new PostgresBookRepository(connectionString);
        _bookRepository.OpenConnection();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _bookRepository.Dispose();
    }

    [Benchmark]
    public void Insert()
    {
        var random = new Random();
        var book = new Book(random.NextInt64(), (random.Next() % 2) + 1, "Benchmark", "Insert", random.Next(2000, 2023));
        _bookRepository.InsertOne(book);
    }


    [Benchmark]
    public long GetCount()
    {
        var random = new Random();
        var year = random.Next(1523, 2000);
        return _bookRepository.GetCount(year);
    }

    [Benchmark]
    public Book? GetById()
    {
        var random = new Random();
        var id = random.Next(1, 1_000_000);
        return _bookRepository.GetById(id);
    }

    [Benchmark]
    public Book? GetByIdAndCategory()
    {
        var random = new Random();
        var id = random.Next(1, 1_000_000);
        return _bookRepository.GetByIdAndCategory(id, id % 2 + 1);
    }
}