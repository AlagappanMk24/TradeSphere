namespace TradeSphere.Application.UseCases.Interfaces
{
    public interface IOrderUseCase
    {
        Task<List<OrderInfoDto>> GetAllOrder();
        Task<OrderInfoDto> GetById(int id);
        Task<List<OrderInfoDto>> GetByUserId(int UserId);
        Task<OrderInfoDto> Checkout(CreateOrderDto dto);
        Task<OrderInfoDto> CancelOrder(int orderId);
        Task<OrderInfoDto> UpdateOrderStatus(UpdateOrderStatusDto orderstatus);
        Task<string> GetStatus(int orderId);
        Task<Order> DeleteOrder(int orderId);
    }
}