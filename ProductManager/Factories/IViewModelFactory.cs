using Domain.Models;
using ProductManager.ViewModels;

namespace ProductManager.Factories;

public interface IViewModelFactory
{
    ProductViewModel CreateProductViewModel(Product product);
}
