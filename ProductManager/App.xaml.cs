using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using ProductManager.ViewModels;
using ProductManager.Views;

namespace ProductManager;

public partial class App : Application
{

    public static IHost? AppHost { get; private set; }

    public App()
    {

        string connectionString = "Server = BADBLUESPC\\TESTSERVER; Database=ProductManagementDb; User Id=sa; Password=root";

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();
                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

                services.AddTransient<IProductRepository, DbProductRepository>();
                services.AddTransient<ILinkRepository, DbLinkRepository>();

            }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        AppHost.Dispose();

        base.OnExit(e);
    }
}

