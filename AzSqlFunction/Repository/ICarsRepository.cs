using AzSqlFunction.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AzSqlFunction.Repository;

public interface ICarsRepository
{
    Task<bool> CreateAsync(Car car, CancellationToken token);
    Task<IEnumerable<Car>> GetAllAsync(CancellationToken token);
    Task<Car> GetCarById(int id, CancellationToken token);
}
