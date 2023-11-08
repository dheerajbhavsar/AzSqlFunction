using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using AzSqlFunction.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AzSqlFunction;

public class SqlFunction
{
    private readonly ILogger<SqlFunction> _logger;
    private readonly IConfiguration _config;

    public SqlFunction(ILogger<SqlFunction> logger, IConfiguration config)
    {
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        _config = config ?? throw new System.ArgumentNullException(nameof(config));
    }

    [FunctionName("CreateCar")]
    public async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Car car)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var id = car.Id;
        _logger.LogInformation("Received Id: {id}", id);

        var query = "INSERT INTO [dbo].[Cars] (Id, Name) VALUES(@id, @name)";

        using var connection = new SqlConnection(_config.GetConnectionString("default"));
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("name", car.Name);
        if (connection.State != ConnectionState.Open)
            connection.Open();

        var _ = command.ExecuteNonQuery();

        await Task.Yield();
        return new OkObjectResult(id);
    }
}
