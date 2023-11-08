using AzSqlFunction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzSqlFunction.Repository;

public interface ICarsRepository
{
    Task<bool> CreateAsync(Car car);
    Task<IEnumerable<Car>> GetAllAsync();
}
