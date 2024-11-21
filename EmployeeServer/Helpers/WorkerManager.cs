using EmployeeServer.Interfaces;
using EmployeeServer.Models;
using System.Collections.Concurrent;

namespace EmployeeServer.Helpers
{
    public class WorkerManager
    {
        private readonly IWorkerService _workerService;
        private readonly ConcurrentDictionary<int, SemaphoreSlim> _workerLocks;

        public WorkerManager(IWorkerService workerService)
        {
            _workerService = workerService;
            _workerLocks = new ConcurrentDictionary<int, SemaphoreSlim>();
        }

        private SemaphoreSlim GetOrCreateLock(int workerId)
        {
            return _workerLocks.GetOrAdd(workerId, _ => new SemaphoreSlim(1, 1));
        }

        public async Task<IEnumerable<Worker>> GetWorkersAsync()
        {
            // Просто возвращаем список сотрудников
            return await _workerService.GetWorkersAsync();
        }

        public async Task AddWorkerAsync(Worker worker)
        {
            await _workerService.AddWorkerAsync(worker);
        }

        public async Task UpdateWorkerAsync(Worker worker)
        {
            var workerLock = GetOrCreateLock(worker.Id);

            await workerLock.WaitAsync(); // Блокировка для конкретного worker.Id
            try
            {
                await _workerService.UpdateWorkerAsync(worker);
            }
            finally
            {
                workerLock.Release(); // Снятие блокировки
            }
        }

        public async Task DeleteWorkerAsync(int workerId)
        {
            var workerLock = GetOrCreateLock(workerId);

            await workerLock.WaitAsync();
            try
            {
                await _workerService.DeleteWorkerAsync(workerId);
            }
            finally
            {
                workerLock.Release();
            }
        }
    }
}
