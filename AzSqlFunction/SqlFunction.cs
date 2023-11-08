using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using AzSqlFunction.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Web.Http;
using System;

namespace AzSqlFunction;

public class SqlFunction
{
    private readonly IConfiguration _config;
    private readonly ILogger<SqlFunction> _logger;

    public SqlFunction(ILogger<SqlFunction> logger, IConfiguration config)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    [FunctionName("Car")]
    public async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Car car)
    {
        var id = car.Id;
        _logger.LogInformation("Received Id: {id}", id);

        var query = "INSERT INTO [dbo].[Cars] (Id, Name) VALUES(@id, @name)";

        try
        {
            using var connection = new SqlConnection(_config.GetConnectionString("default"));
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("name", car.Name);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            var _ = await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            return new ExceptionResult(ex, true);
        }

        return new OkObjectResult(id);
    }
}
