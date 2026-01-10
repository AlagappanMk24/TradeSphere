using TradeSphere.Application.Contracts.DTOs.ShoppingCartDto;

namespace TradeSphere.Api.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class ShoppingCartController(IShoppingCartUseCase shoppingCartUseCase) : ControllerBase
    {
        private readonly IShoppingCartUseCase _shoppingCartUseCase = shoppingCartUseCase;

        // GET api/carts
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartByUserId()
        {
            var userId = GetUserIdFromClaims();
            var shoppingCart = await _shoppingCartUseCase.GetShoppingCartByUserIdAsync(userId);
            if (shoppingCart is null)
                return NotFound(new ApiResponse(404, "Shopping Cart not found"));
            return Ok(shoppingCart);
        }

        // POST api/carts/items
        [HttpPost("items")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartDto>> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = GetUserIdFromClaims();
            var updatedCart = await _shoppingCartUseCase.AddToCartAsync(userId, dto);
            return Ok(updatedCart);
        }

        // PUT api/carts/items/{productId}
        [HttpPut("items/{productId:int}")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartDto>> UpdateCartItemQuantity(int productId, [FromBody] int quantity)
        {
            var userId = GetUserIdFromClaims();
            var updatedCart = await _shoppingCartUseCase.UpdateCart(userId, productId, quantity);
            if (updatedCart is null)
                return NotFound(new ApiResponse(404, "Shopping Cart not found"));
            return Ok(updatedCart);
        }

        // DELETE api/carts/items/{productId}
        [HttpDelete("items/{productId:int}")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartDto>> RemoveFromCart([FromQuery] int productId, [FromQuery] int quantity)
        {
            var userId = GetUserIdFromClaims();
            var updatedCart = await _shoppingCartUseCase.RemoveFromCartAsync(userId, productId, quantity);
            if (updatedCart is null)
                return NotFound(new ApiResponse(404, "Shopping Cart not found"));
            return Ok(updatedCart);
        }

        // DELETE api/carts
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<ShoppingCartDto>> ClearCart()
        {
            var userId = GetUserIdFromClaims();
            var clearedCart = await _shoppingCartUseCase.ClearCartAsync(userId);
            if (clearedCart is null)
                return NotFound(new ApiResponse(404, "Shopping Cart not found"));
            return Ok(clearedCart);
        }
        private int GetUserIdFromClaims()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out var id) ? id : throw new UnauthorizedAccessException("Invalid User");
        }
    }
}