using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Haulmer.Payments.Infrastructure.Persistence;

namespace Haulmer.Payments.Infrastructure.IntegrationTests.Persistence.TestInfrastructure;

public class SqliteInMemoryDbContextFactory : IDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteInMemoryDbContextFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    public PaymentsDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PaymentsDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;
        var context = new PaymentsDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
