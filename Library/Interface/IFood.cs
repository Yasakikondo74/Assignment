using Library.Model;

namespace Library.Interface
{
    public interface IFood
    {
        Task<Food> Find(Guid ID, string name);
        Task<List<Food>> GetList();
        Task Create(Food food);
        Task<bool> Update(Food food, Guid ID);
        Task<bool> Delete(Guid ID);
    }
}
