using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EmployeeClient.Models;
using EmployeeClient.Commands;
using System.Windows.Input;
using EmployeeClient.Services;
using EmployeeContracts;
using System.Windows;

namespace EmployeeClient.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IWorkerClientService _workerClientService;

        public ObservableCollection<WorkerMessage> Workers { get; set; } = new ObservableCollection<WorkerMessage>();

        public AsyncRelayCommand LoadWorkersCommand { get; }
        public AsyncRelayCommand<WorkerMessage> AddWorkerCommand { get; }
        public AsyncRelayCommand<WorkerMessage> UpdateWorkerCommand { get; }
        public AsyncRelayCommand<int> DeleteWorkerCommand { get; }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private WorkerMessage _selectedWorker;
        public WorkerMessage SelectedWorker
        {
            get => _selectedWorker;
            set => SetProperty(ref _selectedWorker, value);
        }

        public MainViewModel(IWorkerClientService workerClientService)
        {
            _workerClientService = workerClientService;

            // Инициализация команд
            LoadWorkersCommand = new AsyncRelayCommand(LoadWorkersAsync);
            AddWorkerCommand = new AsyncRelayCommand<WorkerMessage>(AddWorkerAsync);
            UpdateWorkerCommand = new AsyncRelayCommand<WorkerMessage>(UpdateWorkerAsync);
            DeleteWorkerCommand = new AsyncRelayCommand<int>(DeleteWorkerAsync);
        }

        private async Task LoadWorkersAsync()
        {
            try
            {
                var workers = await _workerClientService.GetWorkersAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Workers.Clear();
                    foreach (var worker in workers)
                    {
                        Workers.Add(worker);
                    }
                });
                StatusMessage = "Workers loaded successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading workers: {ex.Message}";
            }
        }

        private async Task AddWorkerAsync(WorkerMessage worker)
        {
            try
            {
                await _workerClientService.AddWorkerAsync(worker);
                StatusMessage = "Worker added successfully.";
                await LoadWorkersAsync(); // Обновляем список
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error adding worker: {ex.Message}";
            }
        }

        private async Task UpdateWorkerAsync(WorkerMessage worker)
        {
            try
            {
                await _workerClientService.UpdateWorkerAsync(worker);
                StatusMessage = "Worker updated successfully.";
                await LoadWorkersAsync(); // Обновляем список
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating worker: {ex.Message}";
            }
        }

        private async Task DeleteWorkerAsync(int workerId)
        {
            try
            {
                await _workerClientService.DeleteWorkerAsync(workerId);
                StatusMessage = "Worker deleted successfully.";
                await LoadWorkersAsync(); // Обновляем список
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error deleting worker: {ex.Message}";
            }
        }
    }
}
