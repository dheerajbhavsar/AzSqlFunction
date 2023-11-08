using AzSqlFunction.Models;
using System.Threading.Tasks;

namespace AzSqlFunction.Repository;

public interface ICarsRepository
{
    Task<bool> Create(Car car);
}
