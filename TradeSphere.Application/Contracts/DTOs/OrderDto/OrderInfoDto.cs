using TradeSphere.Application.Contracts.DTOs.PaymentDto;
using TradeSphere.Application.DTOs.OrderItemDto;

namespace TradeSphere.Application.DTOs.OrderDto
{
    public class OrderInfoDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string userName { get; set; }
        public PaymenInfoDto PaymentInfo { get; set; }
        public List<OrderItemInfoDto> OrderItems { get; set; }
    }
}
