using System.Configuration;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Persistence.Repositories;
using Persistence.Repositories.Interfaces;
using ProductManager.Factories;
using ProductManager.Services;
using ProductManager.ViewModels;
using ProductManager.Views;

namespace ProductManager;

public partial class App : Application
{

    public static IHost? AppHost { get; private set; }

    public App()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./appsettings.json", optional: false);

        IConfiguration configuration = builder.Build();

        string? connectionString = configuration.GetConnectionString("SQLiteDatabase");

        if (string.IsNullOrEmpty(connectionString))
            throw new ConfigurationErrorsException("Couldn't find DB configuration");

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<AddProductViewModel>();
                services.AddTransient<IViewModelFactory, ViewModelFactory>();
                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseSqlite(connectionString);
                });

                services.AddTransient<IProductRepository, DbProductRepository>();
                services.AddTransient<ILinkRepository, DbLinkRepository>();
                services.AddTransient<ExcelService>();

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

