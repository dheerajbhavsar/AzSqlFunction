using System;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using AzSqlFunction.Models;
using AzSqlFunction.Repository;
using Microsoft.AspNetCore.Http;

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

    [FunctionName("CreateCars")]
    public async Task<IActionResult> CreateAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Car car)
    {
        _logger.LogInformation("Received Id: {id}", car.Id);

        try
        {
            var result = await _repository.CreateAsync(car);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            return new ExceptionResult(ex, true);
        }
    }

    [FunctionName("GetAllCars")]
    public async Task<IActionResult> GetAsync(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request)
    {
        _logger.LogInformation("Getting cars");

        try
        {
            var result = await _repository.GetAllAsync();
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            return new ExceptionResult(ex, true);
        }
    }
}
