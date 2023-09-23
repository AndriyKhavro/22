using SqlDataGenerator.Library;

namespace SqlDataGenerator;

internal class DataGenerator
{
    public List<Book> GenerateBatch(long startId, int batchSize)
    {
        var usersBatch = new List<Book>();
        for (var i = startId; i < startId + batchSize; i++)
        {
            string author = $"Author{i}";
            string title = $"Title{i}";

            usersBatch.Add(new Book(i, (int)(i % 2) + 1, author, title, GetRandomYear()));
        }
        return usersBatch;
    }

    private static int GetRandomYear()
    {
        var random = new Random();
        int currentYear = DateTime.Now.Year;
        int randomYear = random.Next(currentYear - 500, currentYear + 1);

        // Create a random date using the generated year, month, and day
        return randomYear;
    }
}