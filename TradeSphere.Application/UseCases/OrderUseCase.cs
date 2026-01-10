using TradeSphere.Application.DTOs.OrderDto;
using TradeSphere.Application.DTOs.OrderDto.CreateOrderDto;
using TradeSphere.Domain.Enums;

namespace TradeSphere.Application.UseCases
{
    public class OrderUseCase(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper) : IOrderUseCase
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<List<OrderInfoDto>> GetAllOrder()
        {
            var getOrders = await _orderRepository.GetAll();
            var orders = _mapper.Map<List<OrderInfoDto>>(getOrders);
            return orders;
        }
        public async Task<OrderInfoDto> GetById(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order is null) return null;
            var result = _mapper.Map<OrderInfoDto>(order);
            return result;
        }
        public async Task<List<OrderInfoDto>> GetByUserId(int UserId)
        {
            var order = await _orderRepository.GetByUserId(UserId);
            if (order is null) return null;
            var result = _mapper.Map<List<OrderInfoDto>>(order);
            return result;
        }
        public async Task<OrderInfoDto> Checkout(CreateOrderDto dto)
        {

            if (dto == null || dto.OrderItems.Count == 0)
                throw new ArgumentException("Invalid order");

            var order = new Order
            {
                ApplicationUserId = dto.AppUserId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = []
            };

            decimal total = 0;

            foreach (var item in dto.OrderItems)
            {
                var product = await _productRepository.GetById(item.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                if (product.Quantity < item.Quantity)
                    throw new Exception("Insufficient stock");

                product.Quantity -= item.Quantity;
                await _productRepository.UpdateProduct(product);
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                total += orderItem.Quantity * orderItem.UnitPrice;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;

            order.Payment = new Payment
            {
                Amount = total,
                Status = PaymentStatus.Pending,
                ApplicationUserId = dto.AppUserId,
                PaymentDate = DateTime.UtcNow,
            };

            await _orderRepository.AddOrder(order);

            var savedOrder = await _orderRepository.GetById(order.Id);
            return _mapper.Map<OrderInfoDto>(savedOrder);
        }
        public async Task<OrderInfoDto> CancelOrder(int orderId)
        {
            var order = await _orderRepository.GetByIdTracked(orderId) ?? throw new Exception("Order not found");
            if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered)
                throw new Exception("Order is already cancelled");
            order.Status = OrderStatus.Cancelled;
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdTracked(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }
            await _orderRepository.UpdateOrder(order);
            return _mapper.Map<OrderInfoDto>(order);
        }
        public async Task<OrderInfoDto> UpdateOrderStatus(UpdateOrderStatusDto orderstatus)
        {
            var order = await _orderRepository.GetByIdTracked(orderstatus.OrderId);
            if (order == null)
                throw new Exception("Order not found");
            _ = Enum.TryParse<OrderStatus>(orderstatus.Status, out var status);
            order.Status = status;
            await _orderRepository.UpdateOrder(order);
            return _mapper.Map<OrderInfoDto>(order);
        }
        public async Task<string> GetStatus(int orderId)
        {
            var order = await _orderRepository.GetById(orderId) ?? throw new Exception("Order not found");
            return order.Status.ToString();
        }
        public async Task<Order> DeleteOrder(int orderId)
        {
            var order = await _orderRepository.GetByIdTracked(orderId) ?? throw new Exception("Order not found");
            var orderDeleted = await _orderRepository.DeleteOrder(order);
            return orderDeleted;
        }
    }
}