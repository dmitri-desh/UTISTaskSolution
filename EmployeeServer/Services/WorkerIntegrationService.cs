using EmployeeContracts;
using EmployeeServer.Data;
using EmployeeServer.Helpers;
using EmployeeServer.Interfaces;
using EmployeeServer.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Action = EmployeeContracts.Action;
using GetWorkersResponse = EmployeeContracts.GetWorkersResponse;

namespace EmployeeServer.Services
{
    public class WorkerIntegrationService : WorkerIntegration.WorkerIntegrationBase
    {
        private readonly WorkerManager _workerManager;

        public WorkerIntegrationService(WorkerManager workerManager)
        {
            _workerManager = workerManager;
        }

        public override async Task GetWorkerStream(
            EmptyMessage request, 
            IServerStreamWriter<WorkerAction> responseStream, 
            ServerCallContext context)
        {
            var random = new Random();
            while (!context.CancellationToken.IsCancellationRequested)
            {
                // Генерация случайного изменения сотрудника
                var worker = new WorkerMessage
                {
                    LastName = "Vasia",
                    FirstName = "Pupkin",
                    MiddleName = "A.",
                    Birthday = DateTime.UtcNow.AddYears(-random.Next(20, 60)).Ticks,
                    Sex = Sex.Male,
                    HasChildren = random.Next(0, 2) == 1
                };

                var action = (Action)random.Next(1, 4); // Create, Update, Delete

                var workerAction = new WorkerAction
                {
                    Worker = worker,
                    ActionType = action
                };

                // Отправка события клиенту
                await responseStream.WriteAsync(workerAction);

                await Task.Delay(3000); // Имитация задержки
            }
        }

        public override async Task<GetWorkersResponse> GetWorkers(EmptyMessage request, ServerCallContext context)
        {
            var workers = await _workerManager.GetWorkersAsync();
            var response = new GetWorkersResponse();

            response.Workers.AddRange(workers.Select(w => new WorkerMessage
            {
                LastName = w.LastName,
                FirstName = w.FirstName,
                MiddleName = w.MiddleName,
                Birthday = ((DateTimeOffset)w.Birthday).ToUnixTimeSeconds(),
                Sex = w.Sex == "Male" ? Sex.Male : Sex.Female,
                HasChildren = w.HasChildren
            }));

            return response;
        }

        public override async Task<EmptyMessage> AddWorker(WorkerMessage request, ServerCallContext context)
        {
            var worker = new Worker
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                Birthday = DateTimeOffset.FromUnixTimeSeconds(request.Birthday).UtcDateTime,
                Sex = request.Sex == Sex.Male ? "Male" : "Female",
                HasChildren = request.HasChildren
            };

            await _workerManager.AddWorkerAsync(worker);
            return new EmptyMessage();
        }

        public override async Task<EmptyMessage> UpdateWorker(WorkerMessage request, ServerCallContext context)
        {
            var worker = new Worker
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                Birthday = DateTimeOffset.FromUnixTimeSeconds(request.Birthday).UtcDateTime,
                Sex = request.Sex == Sex.Male ? "Male" : "Female",
                HasChildren = request.HasChildren
            };

            await _workerManager.UpdateWorkerAsync(worker);
            return new EmptyMessage();
        }

        public override async Task<EmptyMessage> DeleteWorker(WorkerIdMessage request, ServerCallContext context)
        {
            await _workerManager.DeleteWorkerAsync(request.Id);
            return new EmptyMessage();
        }
    }
}
