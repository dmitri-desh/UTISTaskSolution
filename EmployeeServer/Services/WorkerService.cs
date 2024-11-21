using EmployeeServer.Data;
using EmployeeServer.Interfaces;
using EmployeeServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeServer.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly AppDbContext _dbContext;

        public WorkerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Worker>> GetWorkersAsync()
        {
            return await _dbContext.Workers.AsNoTracking().ToListAsync();
        }

        public async Task AddWorkerAsync(Worker worker)
        {
            await _dbContext.Workers.AddAsync(worker);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateWorkerAsync(Worker worker)
        {
            _dbContext.Workers.Update(worker);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWorkerAsync(int workerId)
        {
            var worker = await _dbContext.Workers.FindAsync(workerId);
            if (worker != null)
            {
                _dbContext.Workers.Remove(worker);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
