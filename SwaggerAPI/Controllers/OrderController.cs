using Library.Interface;
using Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderRepository;

        public OrderController(IOrder orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetList();
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }
            return Ok(orders);
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> Find(Guid ID)
        {
            var result = await _orderRepository.Find(ID);
            if (result == null)
            {
                return NotFound($"Order with ID {ID} not found.");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (order == null)
            {
                return BadRequest("Order data is null.");
            }
            await _orderRepository.Create(order);
            return CreatedAtAction(nameof(Find), new { ID = order.ID }, order);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> Update(Order order, Guid ID)
        {
            if (order == null || ID == Guid.Empty)
            {
                return BadRequest("Order data or ID is invalid.");
            }
            var updated = await _orderRepository.Update(order, ID);
            if (!updated)
            {
                return NotFound($"Order with ID {ID} not found.");
            }
            return NoContent();
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> Delete(Guid ID)
        {
            if (ID == Guid.Empty)
            {
                return BadRequest("ID is invalid.");
            }
            var deleted = await _orderRepository.Delete(ID);
            if (!deleted)
            {
                return NotFound($"Order with ID {ID} not found.");
            }
            return NoContent();
        }
    }
}
