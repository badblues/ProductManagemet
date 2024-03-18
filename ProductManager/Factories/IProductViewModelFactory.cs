using Domain.Models;
using ProductManager.ViewModels;

namespace ProductManager.Factories;

public interface IProductViewModelFactory
{
    ProductViewModel CreateProductViewModel(Product product);
}
