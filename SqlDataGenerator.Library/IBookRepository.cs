namespace SqlDataGenerator.Library;

public interface IBookRepository : IDisposable
{
    void OpenConnection();
    void InsertBatch(IEnumerable<Book> books);
    long GetMaxId();
}