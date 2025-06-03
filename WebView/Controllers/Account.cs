using Library.Interface;
using Library.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace WebView.Controllers
{
    [Route("AccountController")]
    public class Account : Controller
    {
        private readonly IAccount _accountRepository;

        public Account(IAccount accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _accountRepository.Login(username, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                if (user.Role == "Admin")
                {
                    Console.WriteLine($"Logged in user role: {user.Role}");
                    return RedirectToAction("List");
                }
                else
                {
                    Console.WriteLine($"Logged in user role: {user.Role}");
                    return RedirectToAction("CustomerList", "FoodController");
                }
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }
        [HttpPost, Route("Logout"), AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet("List"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var accounts = await _accountRepository.GetList();
            return View(accounts);
        }
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) 
            {
                return BadRequest("Account ID cannot be empty.");
            }
            var account = await _accountRepository.Find(id, ""); 

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
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

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("RegisterAdmin")]
        public IActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("RegisterCustomer")]
        public IActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(Library.Model.Account account)
        {
            if (!ModelState.IsValid)
                return View(account);

            await _accountRepository.Create(account);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomer(Library.Model.Account account)
        {
            if (!ModelState.IsValid)
                return View(account);

            await _accountRepository.Create(account);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("Edit/{ID}")]
        public async Task<IActionResult> Edit(Guid ID)
        {
            var account = await _accountRepository.Find(ID, null);
            if (account == null)
                return NotFound($"Account with ID {ID} not found.");
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Library.Model.Account account, Guid ID)
        {
            if (await _accountRepository.Update(account, ID))
            {
                return RedirectToAction("List");
            }
            return NotFound($"Account with ID {ID} not found.");
        }
        [HttpGet("Delete/{ID}")]
        public async Task<IActionResult> Delete(Guid ID)
        {
            var account = await _accountRepository.Find(ID, null);
            if (account == null)
                return NotFound($"Account with ID {ID} not found.");
            return View(account);
        }
        [HttpPost("Delete/{ID}"), ActionName("Delete")]
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
