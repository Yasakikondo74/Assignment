using Library.Interface;
using Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebView.Controllers
{
    [Route("OrderController")]
    public class Order : Controller
    {
        private readonly IOrder _orderRepos;

        public Order(IOrder orderRepos)
        {
            _orderRepos = orderRepos;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var food = await _orderRepos.GetList();
            return View(food);
        }
        public async Task<IActionResult> Find(Guid? ID)
        {
            if (ID.HasValue)
            {
                var result = await _orderRepos.Find(ID.Value);
                if (result == null)
                    return NotFound($"Account with ID {ID} not found.");
                return View(result);
            }
            return BadRequest("Please provide either an ID or name.");
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Library.Model.Order order)
        {
            await _orderRepos.Create(order);
            return RedirectToAction("List");
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> Update(Guid ID)
        {
            var food = await _orderRepos.Find(ID);
            if (food == null)
                return NotFound($"Food with ID {ID} not found.");
            return View(food);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Library.Model.Order order, Guid ID)
        {
            if (await _orderRepos.Update(order, ID))
            {
                return RedirectToAction("List");
            }
            return NotFound($"Food with ID {ID} not found.");
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> Delete(Guid ID)
        {
            var account = await _orderRepos.Find(ID);
            if (account == null)
                return NotFound($"Account with ID {ID} not found.");
            return View(account);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid ID)
        {
            if (await _orderRepos.Delete(ID))
            {
                return RedirectToAction("List");
            }
            return NotFound($"Account with ID {ID} not found.");
        }
    }
}
