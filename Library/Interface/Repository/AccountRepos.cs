using Library.Model;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace Library.Interface.Repository
{
    public class AccountRepos : IAccount
    {
        private readonly DatabaseContext _context;
        public AccountRepos(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<Account?> Login(string username, string password)
        {
            var user = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Username == username);

            // Since passwords are stored as plain text, compare directly
            if (user != null && user.Password == password)
            {
                return user;
            }

            return null;
        }
        public async Task<Account> Find(Guid ID, string name)
        {
            if (ID != Guid.Empty)
            {
                return await _context.Accounts.FirstOrDefaultAsync(a => a.ID == ID);
            }
            else if (!string.IsNullOrEmpty(name)) 
            {
                return await _context.Accounts.FirstOrDefaultAsync(a => a.FullName == name);
            }
            else
                return null;
        }
        public async Task<List<Account>> GetList()
        {
            return await _context.Accounts.ToListAsync();
        }
        public async Task Create(Account account)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> Update(Account account, Guid ID)
        {
            var ToUpdate = await _context.Accounts.FirstOrDefaultAsync(a => a.ID == ID);
            if (ToUpdate == null) return false;

            ToUpdate.Username = account.Username;
            if (!BCrypt.Net.BCrypt.Verify(account.Password, ToUpdate.Password))
            {
                ToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            }
            ToUpdate.PhoneNumber = account.PhoneNumber;
            ToUpdate.Email = account.Email;
            ToUpdate.FullName = account.FullName;
            ToUpdate.Age = account.Age;
            ToUpdate.Role = account.Role;

            _context.Accounts.Update(ToUpdate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(Guid ID)
        {
            var ToDelete = await _context.Accounts.FirstOrDefaultAsync(a => a.ID == ID);
            if (ToDelete == null) return false;

            _context.Accounts.Remove(ToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
