using EmployeeClient.Models;
using EmployeeContracts;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Services
{
    public class GrpcServiceClient
    {
        private readonly WorkerIntegration.WorkerIntegrationClient _client;

        public GrpcServiceClient(string serverAddress)
        {
            var channel = GrpcChannel.ForAddress(serverAddress);
            _client = new WorkerIntegration.WorkerIntegrationClient(channel);
        }

        public async Task<IEnumerable<WorkerModel>> GetWorkersAsync()
        {
            var employees = new List<WorkerModel>();

            using var call = _client.GetWorkerStream(new EmptyMessage());
            await foreach (var workerAction in call.ResponseStream.ReadAllAsync())
            {
                var worker = workerAction.Worker;
                employees.Add(new WorkerModel
                {
                    LastName = worker.LastName,
                    FirstName = worker.FirstName,
                    MiddleName = worker.MiddleName,
                    Birthday = DateTimeOffset.FromUnixTimeMilliseconds(worker.Birthday).DateTime,
                    Sex = worker.Sex.ToString(),
                    HasChildren = worker.HasChildren
                });
            }

            return employees;
        }
    }
}
