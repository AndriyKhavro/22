namespace PostgresSharding.Benchmark;

internal class ConnectionStrings
{
    public static readonly IReadOnlyDictionary<string, string> All = new Dictionary<string, string>
    {
        { nameof(Fdw), Fdw },
        { nameof(NoSharding), NoSharding },
        { nameof(Citus), Citus},

    };

    public const string Fdw = "Server=localhost;Port=5432;Database=books;Uid=postgres;Pwd=postgres;";
    public const string NoSharding = "Server=localhost;Port=15432;Database=books;Uid=postgres;Pwd=postgres;";
    public const string Citus = "Server=localhost;Port=25432;Uid=postgres;";
}