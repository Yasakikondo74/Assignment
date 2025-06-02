using Library.Model;
using Microsoft.EntityFrameworkCore;

namespace Library.Interface.Repository
{
    public class OrderRepos : IOrder
    {
        private readonly DatabaseContext _context;

        public OrderRepos(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Order?> Find(Guid ID)
        {
            return await _context.Orders
                .Include(o => o.Accounts)
                .Include(o => o.Foods)
                .FirstOrDefaultAsync(o => o.ID == ID);
        }

        public async Task<List<Order>> GetList()
        {
            return await _context.Orders
                .Include(o => o.Accounts)
                .Include(o => o.Foods)
                .ToListAsync();
        }

        public async Task Create(Order order)
        {
            if (order.Accounts != null)
            {
                foreach (var account in order.Accounts)
                {
                    if (account.ID != Guid.Empty)
                    {
                        _context.Entry(account).State = EntityState.Unchanged;
                    }
                    else
                    {
                        _context.Entry(account).State = EntityState.Detached;
                    }
                }
            }
            if (order.Foods != null)
            {
                foreach (var food in order.Foods)
                {
                    if (food.ID != Guid.Empty)
                    {
                        _context.Entry(food).State = EntityState.Unchanged;
                    }
                    else
                    {
                        _context.Entry(food).State = EntityState.Detached;
                    }
                }
            }
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Order order, Guid ID)
        {
            var ToUpdate = await _context.Orders
                .Include(o => o.Foods)
                .Include(o => o.Accounts)
                .FirstOrDefaultAsync(o => o.ID == ID);

            if (ToUpdate == null) return false;

            ToUpdate.AccountID = order.AccountID;
            ToUpdate.FoodID = order.FoodID;
            ToUpdate.Status = order.Status;
            ToUpdate.Foods = order.Foods;
            ToUpdate.Accounts = order.Accounts;

            _context.Orders.Update(ToUpdate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid ID)
        {
            var ToDelete = await _context.Orders.FirstOrDefaultAsync(o => o.ID == ID);
            if (ToDelete == null) return false;

            _context.Orders.Remove(ToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
