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
using System.Threading;

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
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "cars")] Car car, CancellationToken token)
    {
        _logger.LogInformation("Received Id: {id}", car.Id);

        try
        {
            var result = await _repository.CreateAsync(car, token);
            return new OkObjectResult(result);
        }
        catch(Exception ex) when (ex.GetType() == typeof(OperationCanceledException)) {
            _logger.LogError(ex, "Cancellation was requested by user");
            return new ExceptionResult(ex, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating car {id}", car.Id);
            return new ExceptionResult(ex, true);
        }
    }

    [FunctionName("GetAllCars")]
    public async Task<IActionResult> GetAllAsync(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "cars")] HttpRequest request, CancellationToken token)
    {
        _logger.LogInformation("Getting cars");

        try
        {
            var result = await _repository.GetAllAsync(token);
            return new OkObjectResult(result);
        }
        catch (Exception ex) when (ex is OperationCanceledException)
        {
            _logger.LogError(ex, "Cancellation was requested by user");
            return new ExceptionResult(ex, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all the cars...");
            return new ExceptionResult(ex, true);
        }
    }

    [FunctionName("GetCarById")]
    public async Task<IActionResult> GetAsync(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "cars/{id}")] HttpRequest request, int id, CancellationToken token)
    {
        _logger.LogInformation("Getting cars");

        try
        {
            var result = await _repository.GetCarById(id, token);
            if (result is null)
                return new NotFoundObjectResult(id);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting car {id}", id);
            return new ExceptionResult(ex, true);
        }
    }
}
