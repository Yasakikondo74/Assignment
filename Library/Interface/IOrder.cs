using Library.Model;

namespace Library.Interface
{
    public interface IOrder
    {
        Task<Order> Find(Guid ID);
        Task<List<Order>> GetList();
        Task Create(Order order);
        Task<bool> Update(Order order, Guid ID);
        Task<bool> Delete(Guid ID);
    }
}
