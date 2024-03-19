using Domain.Models;
using Persistence.Repositories.Interfaces;
using ProductManager.ViewModels;

namespace ProductManager.Factories;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IProductRepository _productRepository;
    private readonly ILinkRepository _linkRepository;

    public ViewModelFactory(IProductRepository productRepository, ILinkRepository linkRepository)
    {
        _productRepository = productRepository;
        _linkRepository = linkRepository;
    }

    public ProductViewModel CreateProductViewModel(Product product)
    {
        return new ProductViewModel(product, _productRepository, _linkRepository);
    }
}
