using EmployeeContracts;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Services
{
    public class WorkerClientService :IWorkerClientService
    {
        private readonly GrpcChannel _channel;
        private readonly WorkerIntegration.WorkerIntegrationClient _client;

        public WorkerClientService(GrpcChannel channel)
        {
            _channel = channel;
            _client = new WorkerIntegration.WorkerIntegrationClient(_channel);
        }

        public async Task<IEnumerable<WorkerMessage>> GetWorkersAsync()
        {
            var response = await _client.GetWorkersAsync(new EmptyMessage());
            return response.Workers;
        }

        public async Task AddWorkerAsync(WorkerMessage worker)
        {
            await _client.AddWorkerAsync(worker);
        }

        public async Task UpdateWorkerAsync(WorkerMessage worker)
        {
            await _client.UpdateWorkerAsync(worker);
        }

        public async Task DeleteWorkerAsync(int workerId)
        {
            await _client.DeleteWorkerAsync(new WorkerIdMessage { Id = workerId });
        }
    }
}
