using EmployeeContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Services
{
    public interface IWorkerClientService
    {
        Task<IEnumerable<WorkerMessage>> GetWorkersAsync();
        Task AddWorkerAsync(WorkerMessage worker);
        Task UpdateWorkerAsync(WorkerMessage worker);
        Task DeleteWorkerAsync(int workerId);
    }
}
