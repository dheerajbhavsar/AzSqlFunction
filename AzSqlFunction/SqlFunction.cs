using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using AzSqlFunction.Models;
using Microsoft.Extensions.Configuration;
using System.Web.Http;
using System;
using AzSqlFunction.Repository;

namespace AzSqlFunction;

public class SqlFunction
{
    private readonly ICarsRepository _repository;
    private readonly ILogger<SqlFunction> _logger;

    public SqlFunction(ILogger<SqlFunction> logger, ICarsRepository repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [FunctionName("Car")]
    public async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Car car)
    {
        _logger.LogInformation("Received Id: {id}", car.Id);

        try
        {
            var result = await _repository.Create(car);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            return new ExceptionResult(ex, true);
        }
    }
}
