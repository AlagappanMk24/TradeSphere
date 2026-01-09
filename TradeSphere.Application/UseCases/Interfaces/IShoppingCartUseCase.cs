using TradeSphere.Application.Contracts.DTOs.ShoppingCartDto;

namespace TradeSphere.Application.UseCases.Interfaces
{
    public interface IShoppingCartUseCase
    {
        Task<ShoppingCartDto> GetShoppingCartByUserIdAsync(int userId);
        Task<ShoppingCartDto> AddToCartAsync(int userId, AddToCartDto dto);
        Task<ShoppingCartDto> RemoveFromCartAsync(int userId, int productId, int quantity);
        Task<ShoppingCartDto> ClearCartAsync(int userId);
        Task<ShoppingCartDto> UpdateCart(int userId, int productId, int quantity);
    }
}
