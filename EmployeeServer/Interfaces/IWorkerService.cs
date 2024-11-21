using EmployeeServer.Models;

namespace EmployeeServer.Interfaces
{
    public interface IWorkerService
    {
        Task<IEnumerable<Worker>> GetWorkersAsync();
        Task AddWorkerAsync(Worker worker);
        Task UpdateWorkerAsync(Worker worker);
        Task DeleteWorkerAsync(int workerId);
    }
}
