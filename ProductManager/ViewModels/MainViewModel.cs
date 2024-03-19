using System.IO;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Models;
using Persistence;
using ProductManager.Core;
using ProductManager.Factories;
using ProductManager.Services;
using ProductManager.Views;

namespace ProductManager.ViewModels;

public class MainViewModel : ViewModel
{
    public ICommand OpenAddProductWindowCommand {  get; set; }
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

    public IEnumerable<Link> Links
    {
        get => _links;
        set
        {
            _links = value;
            OnPropertyChanged(nameof(Links));
        }
    }

    private readonly IProductRepository _productRepository;
    private readonly ILinkRepository _linkRepository;
    private readonly IViewModelFactory _productVMFactory;
    private readonly AddProductViewModel _addProductViewModel;
    private readonly ExcelService _excelService;

    private IEnumerable<Product> _products;
    private IEnumerable<Link> _links;


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
        _addProductViewModel.AddProductEvent += (sender, args) =>
        {
            LoadProducts();
        };
        _excelService = excelService;

        LoadProducts();
        Links = _linkRepository.GetAll();

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
            productViewModel.DeleteProductEvent += (sender, args) =>
            {
                LoadProducts();
            };
            productViewModel.EditProductEvent += (sender, args) =>
            {
                LoadProducts();
            };
            ProductWindow productWindow = new ProductWindow(productViewModel);
            productWindow.Show();
        }
    }

    public void RemoveLink(object? parameter)
    {
        if (parameter is Link link)
        {
            _linkRepository.Delete(link.UpProductId, link.ProductId);
            LoadProducts();
        }
    }

    public void OpenAddProductWindow(object? unused)
    {
        AddProductWindow addProductWindow = new AddProductWindow(_addProductViewModel);
        addProductWindow.Show();
    }

    public void ExportToExcel(object? parameter)
    {
        if (parameter is string filePath)
        {
            try
            {
                XLWorkbook workbook = _excelService.ExportProducts(Products);
                workbook.SaveAs(filePath);
            } catch (IOException)
            {
                MessageBox.Show("Error during file saving", "Error", MessageBoxButton.OK);
            }
        }
    }

    private void LoadProducts()
    {
        Products = _productRepository.GetAll();
        Products = Products.Where(product => product.UpProducts.Count == 0);
    }
}

