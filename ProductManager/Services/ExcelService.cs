using ClosedXML.Excel;
using Domain.Models;

namespace ProductManager.Services;

public class ExcelService
{

    public XLWorkbook ExportProducts(IEnumerable<Product> products, int maxLevel)
    {
        XLWorkbook workbook = new();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Products");

        worksheet.Cell(1, 1).Value = "Product";
        worksheet.Cell(1, 2).Value = "Level";
        worksheet.Cell(1, 3).Value = "Count";
        worksheet.Cell(1, 4).Value = "Cost";
        worksheet.Cell(1, 5).Value = "Price";
        worksheet.Cell(1, 6).Value = "Total count";

        WriteProducts(worksheet, products, maxLevel);

        return workbook;
    }

    private void WriteProducts(IXLWorksheet worksheet, IEnumerable<Product> products, int maxLevel = 5)
    {
        int row = 2;

        foreach (Product product in products)
        {
            //Filter only first-level products
            if (product.UpProducts.Count == 0)
            {
                float totalCost = product.Price;
                int totalCount = 0;
                int myrow = row;
                row += 1;
                foreach (Link subProduct in product.ProductsBelow)
                {
                    row = WriteProductWithSubproducts(worksheet, subProduct, row, 2, maxLevel);
                    totalCost += GetCost(subProduct);
                    totalCount += GetTotalCount(subProduct) + subProduct.Count;
                }

                WriteValues(worksheet, row, product.Name, 1, 1, totalCost, product.Price, totalCount);
            }
        }
    }

    private int WriteProductWithSubproducts(IXLWorksheet worksheet, Link link, int row, int level, int maxLevel)
    {
        if (level > maxLevel)
            return row;

        if (link.Product == null)
            throw new NullReferenceException("One of the links has no Product");

        WriteValues(worksheet, row, link.Product.Name, level, link.Count, GetCost(link), link.Product.Price, GetTotalCount(link));

        row += 1;
        foreach (Link subProduct in link.Product.ProductsBelow)
        {
            row = WriteProductWithSubproducts(worksheet, subProduct, row, level + 1, maxLevel);
        }

        return row;
    }

    private float GetCost(Link link)
    {
        if (link.Product != null)
        {
            float totalCost = link.Product.Price * link.Count;
            foreach (Link subProduct in link.Product.ProductsBelow)
            {
                totalCost += GetCost(subProduct);
            }
            return totalCost;
        }
        return 0;
    }

    private int GetTotalCount(Link link)
    {
        int totalCount = 0;
        if (link.Product != null)
        {
            foreach (Link subProduct in link.Product.ProductsBelow)
            {
                totalCount += GetTotalCount(subProduct) + (subProduct.Count * link.Count);
            }
        }
        return totalCount;
    }

    private void WriteValues(
        IXLWorksheet worksheet,
        int row,
        string name,
        int level,
        int count,
        float totalCost,
        float price,
        int totalCount)
    {
        worksheet.Cell(row, 1).Value = name;
        worksheet.Cell(row, 2).Value = level;
        worksheet.Cell(row, 3).Value = count;
        worksheet.Cell(row, 4).Value = totalCost;
        worksheet.Cell(row, 5).Value = price;
        worksheet.Cell(row, 6).Value = totalCount;
    }
}
