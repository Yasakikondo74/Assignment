using Library.Interface;
using Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebView.Controllers
{
    public class FoodController : Controller
    {
        private readonly IFood _foodrepos;

        public FoodController(IFood foodrepos)
        {
            _foodrepos = foodrepos;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _foodrepos.GetList());
        }
        [HttpGet]
        public async Task<IActionResult> CustomerList()
        {
            var foods = await _foodrepos.GetList();
            return View(foods);
        }
        [HttpGet]
        public async Task<IActionResult> Find(Guid? ID, string name)
        {
            if (ID.HasValue)
            {
                var result = await _foodrepos.Find(ID.Value, null);
                if (result == null)
                    return NotFound($"Account with ID {ID} not found.");
                return View(result);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await _foodrepos.Find(Guid.Empty, name);
                if (result == null)
                    return NotFound($"Account with name {name} not found.");
                return View(result);
            }
            return BadRequest("Please provide either an ID or name.");
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Food food)
        {
            await _foodrepos.Create(food);
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid ID)
        {
            var food = await _foodrepos.Find(ID, null);
            if (food == null)
                return NotFound($"Food with ID {ID} not found.");
            return View(food);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Food food, Guid ID)
        {
            if (await _foodrepos.Update(food, ID))
            {
                return RedirectToAction("List");
            }
            return NotFound($"Food with ID {ID} not found.");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid ID)
        {
            var account = await _foodrepos.Find(ID, null);
            if (account == null)
                return NotFound($"Account with ID {ID} not found.");
            return View(account);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid ID)
        {
            if (await _foodrepos.Delete(ID))
            {
                return RedirectToAction("List");
            }
            return NotFound($"Account with ID {ID} not found.");
        }
    }
}
