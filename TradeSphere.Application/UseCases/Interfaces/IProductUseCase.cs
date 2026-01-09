namespace TradeSphere.Application.UseCases.Interfaces
{
    public interface IProductUseCase
    {
        Task<List<ProductInfoDto>?> GetAllProduct();
        Task<ProductInfoDto?> GetProductById(int id);
        Task<ProductInfoDto?> GetProductByName(string name);
        Task<ProductInfoDto?> AddProduct(ProductAddDto productAdd);
        Task<ProductInfoDto?> UpdateProduct(int id, ProductAddDto updateProductDto);
        Task<bool> DeleteProduct(int id);
    }
}