namespace TradeSphere.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController(IOrderUseCase orderUseCase) : ControllerBase
    {
        private readonly IOrderUseCase _orderUseCase = orderUseCase;

        [HttpGet]
        public async Task<ActionResult<List<OrderInfoDto>>> GetAll()
        {
            var orders = await _orderUseCase.GetAllOrder();
            if (orders is null) return NotFound(new ApiResponse(404));
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderInfoDto>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400, "Invalid Id"));
            var order = await _orderUseCase.GetById(id);
            if (order is null) return NotFound(new ApiResponse(404));
            return Ok(order);
        }

        [HttpGet("users/{userId:int}")]
        public async Task<ActionResult<List<OrderInfoDto>>> GetByUserId(int userId)
        {
            var order = await _orderUseCase.GetByUserId(userId);
            if (order is null) return NotFound(new ApiResponse(404));
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderInfoDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data"));

            if (createOrderDto.AppUserId <= 0)
                return BadRequest(new ApiResponse(400, "Invalid User Id"));

            if (createOrderDto.OrderItems == null || createOrderDto.OrderItems.Count == 0)
                return BadRequest(new ApiResponse(400, "Order must contain at least one item"));

            var order = await _orderUseCase.Checkout(createOrderDto);

            if (order == null)
                return BadRequest(new ApiResponse(400, "Failed to create order"));

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<ActionResult> CancelOrder(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400, "Invalid Id"));
            var result = await _orderUseCase.CancelOrder(id);
            if (result is null)
                return BadRequest(new ApiResponse(400, "Failed to cancel order"));
            return NoContent();
        }

        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data"));
            var result = await _orderUseCase.UpdateOrderStatus(updateOrderStatusDto);
            if (result is null)
                return BadRequest(new ApiResponse(400, "Failed to update order status"));
            return NoContent();
        }

        [HttpGet("{id:int}/status")]
        public async Task<ActionResult<ApiResponse>> GetOrderStatuses(int orderId)
        {
            var statuse = await _orderUseCase.GetStatus(orderId);
            return Ok(new ApiResponse(200, statuse));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400, "Invalid Id"));
            var result = await _orderUseCase.DeleteOrder(id);
            if (result is null)
                return BadRequest(new ApiResponse(400, "Failed to delete order"));
            return NoContent();
        }
    }
}