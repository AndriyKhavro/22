using Npgsql;
using NpgsqlTypes;

namespace SqlDataGenerator.Library;

public sealed class PostgresBookRepository : IBookRepository
{
    private readonly NpgsqlConnection _connection;
    private bool _isConnectionOpen;

    public PostgresBookRepository(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public void OpenConnection()
    {
        _connection.Open();

        _isConnectionOpen = true;

        Console.WriteLine($"Connection opened. Connection Timeout: {_connection.ConnectionTimeout}.");
    }

    public long GetMaxId()
    {
        EnsureConnectionOpened();

        using var command = CreateGetMaxIdCommand();

        var result = command.ExecuteScalar();
        return result is null or DBNull ? 0 : (long)result;
    }

    public Book? GetById(long id)
    {
        EnsureConnectionOpened();

        using var command = CreateGetByIdCommand(id);

        using NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Book(
                Id: (long)reader["id"],
                CategoryId: (int)reader["category_id"],
                Author: (string)reader["author"],
                Title: (string)reader["title"],
                Year: (int)reader["year"]);
        }

        // No book with the given ID found
        return null;
    }

    public Book? GetByIdAndCategory(long id, int categoryId)
    {
        EnsureConnectionOpened();

        using var command = CreateGetByIdAndCategoryCommand(id, categoryId);

        using NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Book(
                Id: (long)reader["id"],
                CategoryId: (int)reader["category_id"],
                Author: (string)reader["author"],
                Title: (string)reader["title"],
                Year: (int)reader["year"]);
        }

        // No book with the given ID found
        return null;
    }

    public long GetCount(int year)
    {
        EnsureConnectionOpened();

        using var command = CreateGetCountCommand(year);

        var result = command.ExecuteScalar();
        return result is null or DBNull ? 0 : (long)result;
    }

    public void InsertBatch(IEnumerable<Book> books)
    {
        EnsureConnectionOpened();

        using var transaction = _connection.BeginTransaction();
        using var command = CreateInsertCommand();
        command.Transaction = transaction;

        foreach (var book in books)
        {
            SetBook(command, book);

            command.ExecuteNonQuery();
        }

        transaction.Commit();
    }

    public void InsertOne(Book book)
    {
        EnsureConnectionOpened();

        using var command = CreateInsertCommand();
        SetBook(command, book);
        command.ExecuteNonQuery();
    }


    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    private void EnsureConnectionOpened()
    {
        if (!_isConnectionOpen)
        {
            OpenConnection();
        }
    }

    private static void SetBook(NpgsqlCommand command, Book book)
    {
        command.Parameters["@id"].Value = book.Id;
        command.Parameters["@category_id"].Value = book.CategoryId;
        command.Parameters["@author"].Value = book.Author;
        command.Parameters["@title"].Value = book.Title;
        command.Parameters["@year"].Value = book.Year;
    }

    private NpgsqlCommand CreateGetMaxIdCommand()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT MAX(id) FROM books";

        return command;
    }

    private NpgsqlCommand CreateGetCountCommand(int year)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM books WHERE id = @id";
        command.Parameters.Add("@year", NpgsqlDbType.Integer);
        command.Parameters["@year"].Value = year;

        return command;
    }

    private NpgsqlCommand CreateGetByIdCommand(long id)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM books WHERE id = @id";
        command.Parameters.Add("@id", NpgsqlDbType.Bigint);
        command.Parameters["@id"].Value = id;

        return command;
    }

    private NpgsqlCommand CreateGetByIdAndCategoryCommand(long id, int categoryId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM books WHERE id = @id AND category_id = @category_id";
        command.Parameters.Add("@id", NpgsqlDbType.Bigint);
        command.Parameters.Add("@category_id", NpgsqlDbType.Integer);
        
        command.Parameters["@id"].Value = id;
        command.Parameters["@category_id"].Value = categoryId;

        return command;
    }

    private NpgsqlCommand CreateInsertCommand()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO books (id, category_id, author, title, year) " +
                              "VALUES (@id, @category_id, @author, @title, @year)";

        command.Parameters.Add("@id", NpgsqlDbType.Bigint);
        command.Parameters.Add("@category_id", NpgsqlDbType.Integer);
        command.Parameters.Add("@author", NpgsqlDbType.Varchar);
        command.Parameters.Add("@title", NpgsqlDbType.Varchar);
        command.Parameters.Add("@year", NpgsqlDbType.Integer);
        return command;
    }
}