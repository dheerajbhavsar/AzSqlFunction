using System.Data;
using AzSqlFunction.Models;
using System.Threading.Tasks;
using AzSqlFunction.Data;
using System;
using Dapper;
using System.Collections.Generic;
using System.Threading;

namespace AzSqlFunction.Repository;

public class CarsRepository : ICarsRepository
{
    private readonly DapperContext _context;

    public CarsRepository(DapperContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> CreateAsync(Car car, CancellationToken token)
    {
        var query = "INSERT INTO [dbo].[Cars] (Id, Name) VALUES (@id, @name)";

        try
        {
            using var connection = _context.CreateConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();
            var insertedRows = await connection.ExecuteAsync(new CommandDefinition(query, car, cancellationToken: token));

            return insertedRows > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Car>> GetAllAsync(CancellationToken token)
    {
        var query = "SELECT TOP 100 * FROM [dbo].[Cars]";
        try
        {
            using var connection = _context.CreateConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            //await Task.Delay(500, token);
            var cars = await connection.QueryAsync<Car>(new CommandDefinition(query, cancellationToken: token));
            return cars;
        }
        catch(OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Car> GetCarById(int id, CancellationToken token)
    {
        var query = "SELECT TOP 1 * FROM [dbo].[Cars] WHERE Id = @Id";
        try
        {
            using var connection = _context.CreateConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var car = await connection.QueryFirstOrDefaultAsync<Car>(new CommandDefinition(query, new { Id = id }, cancellationToken: token));
            return car;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
