using Library.Interface;
using Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFood _foodRepository;

        public FoodController(IFood foodRepository)
        {
            _foodRepository = foodRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _foodRepository.GetList();
            if (foods == null || !foods.Any())
            {
                return NotFound("No food items found.");
            }
            return Ok(foods);
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> Find(Guid? ID, string name)
        {
            if (ID.HasValue)
            {
                var result = await _foodRepository.Find(ID.Value, null);
                if (result == null)
                    return NotFound($"Food with ID {ID} not found.");
                return Ok(result);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await _foodRepository.Find(Guid.Empty, name);
                if (result == null)
                    return NotFound($"Food with name '{name}' not found.");
                return Ok(result);
            }

            return BadRequest("Please provide either an ID or name.");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Food food)
        {
            if (food == null)
            {
                return BadRequest("Food data is null.");
            }
            await _foodRepository.Create(food);
            return CreatedAtAction(nameof(Find), new { ID = food.ID }, food);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> Update(Food food, Guid ID)
        {
            if (food == null || ID == Guid.Empty)
            {
                return BadRequest("Food data or ID is invalid.");
            }
            var updated = await _foodRepository.Update(food, ID);
            if (!updated)
            {
                return NotFound($"Food with ID {ID} not found.");
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
            var deleted = await _foodRepository.Delete(ID);
            if (!deleted)
            {
                return NotFound($"Food with ID {ID} not found.");
            }
            return NoContent();
        }
    }
}
