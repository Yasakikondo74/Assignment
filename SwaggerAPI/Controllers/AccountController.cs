using Library.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _accountRepository;

        public AccountController(IAccount accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountRepository.GetList();

            if (accounts == null || accounts.Count == 0)
            {
                return NotFound("No accounts found.");
            }
            return Ok(accounts);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> Find(Guid? ID, string name)
        {
            if (ID.HasValue)
            {
                var result = await _accountRepository.Find(ID.Value, null);
                if (result == null)
                    return NotFound($"Account with ID {ID} not found.");
                return Ok(result);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await _accountRepository.Find(Guid.Empty, name);
                if (result == null)
                    return NotFound($"Account with name {name} not found.");
                return Ok(result);
            }

            return BadRequest("Please provide either an ID or name.");
        }
        [HttpPost]
        public async Task<IActionResult> Create(Library.Model.Account account)
        {
            if (account == null)
            {
                return BadRequest("Account data is null.");
            }
            await _accountRepository.Create(account);
            return CreatedAtAction(nameof(Find), new { ID = account.ID }, account);
        }
        [HttpPut("{ID}")]
        public async Task<IActionResult> Update(Library.Model.Account account, Guid ID)
        {
            if (account == null || ID == Guid.Empty)
            {
                return BadRequest("Account data or ID is invalid.");
            }
            var updated = await _accountRepository.Update(account, ID);
            if (!updated)
            {
                return NotFound($"Account with ID {ID} not found.");
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
            var deleted = await _accountRepository.Delete(ID);
            if (!deleted)
            {
                return NotFound($"Account with ID {ID} not found.");
            }
            return NoContent();
        }
    }
}
