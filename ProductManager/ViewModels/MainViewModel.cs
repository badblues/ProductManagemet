using System.Windows;
using System.Windows.Input;
using ClosedXML.Excel;
using Domain.Models;
using Persistence.Repositories.Interfaces;
using ProductManager.Commands;
using ProductManager.Core;
using ProductManager.Factories;
using ProductManager.Services;
using ProductManager.Views;

namespace ProductManager.ViewModels;

public class MainViewModel : ViewModel
{
    public ICommand OpenAddProductWindowCommand { get; set; }
    public ICommand SelectProductCommand { get; init; }
    public ICommand RemoveLinkCommand { get; init; }
    public ICommand ExportToExcelCommand { get; init; }

    public IEnumerable<Product> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged(nameof(Products));
        }
    }

    private readonly IProductRepository _productRepository;
    private readonly ILinkRepository _linkRepository;
    private readonly IViewModelFactory _productVMFactory;
    private readonly AddProductViewModel _addProductViewModel;
    private readonly ExcelService _excelService;

    private IEnumerable<Product> _products = new List<Product>();

    public MainViewModel(
        ILinkRepository linkRepository,
        IProductRepository productRepository,
        IViewModelFactory productVMFactory,
        AddProductViewModel addProductViewModel,
        ExcelService excelService)
    {
        _productRepository = productRepository;
        _linkRepository = linkRepository;
        _productVMFactory = productVMFactory;
        _addProductViewModel = addProductViewModel;
        _excelService = excelService;

        _addProductViewModel.AddProductEvent += (sender, args) => LoadProducts();
        LoadProducts();

        SelectProductCommand = new RelayCommand(SelectProduct, o => true);
        RemoveLinkCommand = new RelayCommand(RemoveLink, o => true);
        OpenAddProductWindowCommand = new RelayCommand(OpenAddProductWindow, o => true);
        ExportToExcelCommand = new RelayCommand(ExportToExcel, o => true);
    }

    public void SelectProduct(object? parameter)
    {
        if (parameter is Product product)
        {
            ProductViewModel productViewModel = _productVMFactory.CreateProductViewModel(product);
            productViewModel.DeleteProductEvent += (sender, args) => LoadProducts();
            productViewModel.EditProductEvent += (sender, args) => LoadProducts();

            ProductWindow productWindow = new(productViewModel);
            productWindow.Show();
        }
    }

    public void RemoveLink(object? parameter)
    {
        if (parameter is Link link)
        {
            try
            {
                _linkRepository.Delete(link.UpProductId, link.ProductId);
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public void OpenAddProductWindow(object? unused)
    {
        AddProductWindow addProductWindow = new(_addProductViewModel);
        addProductWindow.Show();
    }

    public void ExportToExcel(object? parameter)
    {
        if (parameter is ProductsExportArgs args)
        {
            try
            {
                XLWorkbook workbook = _excelService.ExportProducts(Products, args.MaxLevel);
                workbook.SaveAs(args.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during export: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }

    private void LoadProducts()
    {
        Products = _productRepository.GetAll();
        //Filter first-level products
        Products = Products.Where(product => product.UpProducts.Count == 0);
    }
}

