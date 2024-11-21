using EmployeeClient.Services;
using EmployeeClient.ViewModels;
using EmployeeClient.Views;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace EmployeeClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Настройка DI контейнера
            var serviceProvider = new ServiceCollection()
                 .AddSingleton(services =>
                 {
                     var channel = GrpcChannel.ForAddress("https://localhost:5000"); // Используйте адрес вашего gRPC сервера
                     return channel;
                 })
                .AddSingleton<IWorkerClientService, WorkerClientService>()
                .AddSingleton<MainViewModel>()
                .AddSingleton<MainWindow>()
                .BuildServiceProvider();

            // Запуск главного окна с зависимостями
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
