using System.Windows;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace WpfApp;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        string connectionString = "Server = BADBLUESPC\\TESTSERVER; Database=ProductManagementDb; User Id=sa; Password=root";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlServer(connectionString);

        ApplicationContext context = new ApplicationContext(optionsBuilder.Options);

        DbProductRepository productRepository = new DbProductRepository(context);

        IEnumerable<Product> products = productRepository.GetAll();
        foreach (var product in products)
        {
            Console.WriteLine(product.Name);
        }
    }
}

