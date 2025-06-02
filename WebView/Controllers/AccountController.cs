using Library.Interface;
using Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebView.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccount _accountRepository;

        public AccountController(IAccount accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var accounts = await _accountRepository.GetList();
            return View(accounts);
        }
        public async Task<IActionResult> Find(Guid? ID, string name)
        {
            if (ID.HasValue)
            {
                var result = await _accountRepository.Find(ID.Value, null);
                if (result == null)
                    return NotFound($"Account with ID {ID} not found.");
                return View(result);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await _accountRepository.Find(Guid.Empty, name);
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
        public async Task<IActionResult> Create(Account account)
        {
            await _accountRepository.Create(account);
            return RedirectToAction("List");
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> Update(Guid ID)
        {
            var account = await _accountRepository.Find(ID, null);
            if (account == null)
                return NotFound($"Account with ID {ID} not found.");
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Account account, Guid ID)
        {
            if (await _accountRepository.Update(account, ID))
            {
                return RedirectToAction("List");
            }
            return NotFound($"Account with ID {ID} not found.");
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> Delete(Guid ID)
        {
            var account = await _accountRepository.Find(ID, null);
            if (account == null)
                return NotFound($"Account with ID {ID} not found.");
            return View(account);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid ID)
        {
            if (await _accountRepository.Delete(ID))
            {
                return RedirectToAction("List");
            }
            return NotFound($"Account with ID {ID} not found.");
        }
    }
}
