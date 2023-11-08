using System.Data;
using AzSqlFunction.Models;
using System.Threading.Tasks;
using AzSqlFunction.Data;
using System;
using Dapper;

namespace AzSqlFunction.Repository;

public class CarsRepository : ICarsRepository
{
    private readonly DapperContext _context;

    public CarsRepository(DapperContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> Create(Car car)
    {
        var query = "INSERT INTO [dbo].[Cars] (Id, Name) VALUES (@id, @name)";

        try
        {
            using var connection = _context.CreateConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();
            var insertedRows = await connection.ExecuteAsync(query, car);

            return insertedRows > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
