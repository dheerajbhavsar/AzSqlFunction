using AzSqlFunction.Data;
using AzSqlFunction.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using AzSqlFunction;

[assembly:FunctionsStartup(typeof(Startup))]
namespace AzSqlFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<DapperContext>();
        builder.Services.AddScoped<ICarsRepository, CarsRepository>();
    }
}
