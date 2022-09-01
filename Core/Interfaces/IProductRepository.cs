using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    //Abstracting to make the ProductController simple and easy to test and maintain 
    public interface IProductRepository
    {
        // Task<IReadOnlyList<Product>> GetProductsAsync();

        // Task<Product> GetProductByIdAsync(int id);
        // Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync();
        // Task<IReadOnlyList<ProductType>> GetProductsTypesAsync();
    }
}