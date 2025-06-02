using Library.Model;
using Microsoft.EntityFrameworkCore;
namespace Library.Interface.Repository
{
    public class FoodRepos : IFood
    {
        private readonly DatabaseContext _context;

        public FoodRepos(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Food?> Find(Guid ID, string name)
        {
            if (ID != Guid.Empty)
                return await _context.Foods.FirstOrDefaultAsync(f => f.ID == ID);

            if (!string.IsNullOrWhiteSpace(name))
                return await _context.Foods.FirstOrDefaultAsync(f => f.Name == name);

            return null;
        }

        public async Task<List<Food>> GetList()
        {
            return await _context.Foods.ToListAsync();
        }

        public async Task Create(Food food)
        {
            await _context.Foods.AddAsync(food);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Food food, Guid ID)
        {
            var ToUpdate = await _context.Foods.FirstOrDefaultAsync(f => f.ID == ID);
            if (ToUpdate == null) return false;

            ToUpdate.Name = food.Name;
            ToUpdate.Description = food.Description;
            ToUpdate.Price = food.Price;
            ToUpdate.Quantity = food.Quantity;
            ToUpdate.ImageUrl = food.ImageUrl;

            _context.Foods.Update(ToUpdate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid ID)
        {
            var ToDelete = await _context.Foods.FirstOrDefaultAsync(f => f.ID == ID);
            if (ToDelete == null) return false;

            _context.Foods.Remove(ToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
