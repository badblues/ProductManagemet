
using ClosedXML.Excel;
using Domain.Models;

namespace ProductManager.Services;

public class ExcelService
{
    public XLWorkbook ExportProducts(IEnumerable<Product> products)
    {
        XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Products");

        worksheet.Cell(1, 1).Value = "Product";
        worksheet.Cell(1, 2).Value = "Level";
        worksheet.Cell(1, 3).Value = "Count";
        worksheet.Cell(1, 4).Value = "Cost";
        worksheet.Cell(1, 5).Value = "Price";
        worksheet.Cell(1, 6).Value = "Total count";

        int row = 2;

        foreach (Product product in products)
        {
            if (product.UpProducts.Count == 0)
            {
                float totalCost = product.Price;
                int totalCount = 0;
                int myrow = row;
                row += 1;
                foreach (Link subProduct in product.ProductsBelow)
                {
                    row = WriteProduct(worksheet, subProduct, row, 2);
                    totalCost += GetCost(subProduct);
                    totalCount += GetTotalCount(subProduct) + subProduct.Count * 1;
                }

                worksheet.Cell(myrow, 1).Value = product.Name;
                worksheet.Cell(myrow, 2).Value = 1;
                worksheet.Cell(myrow, 3).Value = 1;
                worksheet.Cell(myrow, 4).Value = totalCost;
                worksheet.Cell(myrow, 5).Value = product.Price;
                worksheet.Cell(myrow, 6).Value = totalCount;
            }
            
        }
        return workbook;
    }

    private int WriteProduct(IXLWorksheet worksheet, Link link, int row, int level)
    {
        worksheet.Cell(row, 1).Value = link.Product.Name;
        worksheet.Cell(row, 2).Value = level;
        worksheet.Cell(row, 3).Value = link.Count;
        worksheet.Cell(row, 4).Value = GetCost(link);
        worksheet.Cell(row, 5).Value = link.Product.Price;
        worksheet.Cell(row, 6).Value = GetTotalCount(link);

        row += 1;
        foreach (Link subProduct in link.Product.ProductsBelow)
        {
            row = WriteProduct(worksheet, subProduct, row, level + 1);
        }

        return row;
    }

    private float GetCost(Link link)
    {
        float totalCost = link.Product.Price * link.Count;
        foreach (Link subProduct in link.Product.ProductsBelow)
        {
            totalCost += GetCost(subProduct);
        }
        return totalCost;
    }

    private int GetTotalCount(Link link)
    {
        int totalCount = 0;
        foreach (Link subProduct in link.Product.ProductsBelow)
        {
            totalCount += GetTotalCount(subProduct) + subProduct.Count * link.Count;
        }
        return totalCount;
    }
}
