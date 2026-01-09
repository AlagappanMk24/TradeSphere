namespace TradeSphere.API.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ProductController(IProductUseCase productUseCase, ILogger<ProductController> logger) : ControllerBase
    {
        private readonly IProductUseCase _productUseCase = productUseCase;
        private readonly ILogger<ProductController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<List<ProductInfoDto>>> GetAllProduct()
        {
            var products = await _productUseCase.GetAllProduct();
            if (products is null) return NotFound();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductInfoDto>> GetProductById(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400, "InvalidId"));
            var product = await _productUseCase.GetProductById(id);
            if (product is null) return NotFound(new ApiResponse(404));
            return Ok(product);
        }

        [HttpGet("GetByName{name}")]
        public async Task<ActionResult<ProductInfoDto>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest(new ApiResponse(400, "Invalid Name"));
            var product = await _productUseCase.GetProductByName(name);
            if (product is null) return NotFound(new ApiResponse(404));
            return Ok(product);

        }

        [HttpPost]
        public async Task<ActionResult<ProductInfoDto>> AddProduct(ProductAddDto productAdd)
        {
            if (productAdd is null)
            {
                _logger.LogWarning("Model Of ProductAdd Dto Is null ");
                return BadRequest(new ApiResponse(400, message: "Invalid Data"));
            }

            var addProduct = await _productUseCase.AddProduct(productAdd);
            if (addProduct is null)
            {
                _logger.LogError($"Cant Add Product With Name {productAdd.Name}");
                return BadRequest(new ApiResponse(400));
            }
            return Ok(addProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductInfoDto>> UpdateProduct(int id, ProductAddDto updateProduct)
        {
            if (id <= 0 || updateProduct is null)
                return BadRequest(new ApiResponse(400, message: "Invalid Data"));
            var product = await _productUseCase.UpdateProduct(id, updateProduct);
            if (product is null)
                return NotFound(new ApiResponse(404, message: "Product not found or cant update"));
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse(400, "Invalid product id"));

            var deleted = await _productUseCase.DeleteProduct(id);

            if (!deleted)
                return NotFound(new ApiResponse(404, "Product not found"));

            return NoContent();
        }
    }
}
