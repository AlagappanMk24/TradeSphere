namespace TradeSphere.Application.UseCases
{
    public class ProductUseCase(IProductRepository productRepository, IMapper mapper) : IProductUseCase
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<ProductInfoDto?> AddProduct(ProductAddDto productAdd)
        {
            var product = _mapper.Map<Product>(productAdd);
            _ = await _productRepository.AddProduct(product);
            var productInfo = await _productRepository.GetById(product.Id);
            if (productInfo is null) return null;
            var result = _mapper.Map<ProductInfoDto>(productInfo);
            return result;

        }
        public async Task<bool> DeleteProduct(int id) => await _productRepository.DeleteProduct(id);
        public async Task<List<ProductInfoDto>?> GetAllProduct()
        {
            var products = await _productRepository.GetAllProducts();
            var mapProducts = _mapper.Map<List<ProductInfoDto>>(products);
            return mapProducts;
        }
        public async Task<ProductInfoDto?> GetProductById(int id)
        {
            var product = await _productRepository.GetById(id);
            return product is null ? null : _mapper.Map<ProductInfoDto>(product);
        }
        public async Task<ProductInfoDto?> GetProductByName(string name)
        {
            var product = await _productRepository.GetByName(name);
            if (product is null) return null;
            return _mapper.Map<ProductInfoDto>(product);
        }
        public async Task<ProductInfoDto?> UpdateProduct(int id, ProductAddDto updateProductDto)
        {
            var getProductInfo = await _productRepository.GetById(id);
            _mapper.Map(updateProductDto, getProductInfo);
            var updateproduct = await _productRepository.UpdateProduct(getProductInfo);
            if (updateproduct is null) return null;
            var productInfo = _mapper.Map<ProductInfoDto>(updateproduct);
            return productInfo;
        }
    }
}