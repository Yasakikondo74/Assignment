using Library.Model;

namespace Library.Interface
{
    public interface IAccount
    {
        Task<Account> Find(Guid ID, string name);
        Task<List<Account>> GetList();
        Task Create(Account account);
        Task<bool> Update(Account account, Guid ID);
        Task<bool> Delete(Guid ID);
        //login
        Task<Account?> Find_v2(string username, string password);

    }
}
