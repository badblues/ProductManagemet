using Domain.Models;
using Persistence;
using ProductManager.ViewModels;

namespace ProductManager.Factories;

public class ProductViewModelFactory : IProductViewModelFactory
{
    private readonly IProductRepository _productRepository;
    private readonly ILinkRepository _linkRepository;

    public ProductViewModelFactory(IProductRepository productRepository, ILinkRepository linkRepository)
    {
        _productRepository = productRepository;
        _linkRepository = linkRepository;
    }

    public ProductViewModel CreateProductViewModel(Product product)
    {
        return new ProductViewModel(product, _productRepository, _linkRepository);
    }
}
