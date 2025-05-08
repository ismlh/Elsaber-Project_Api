

namespace BL.IRepositories
{
   public interface ClientOrderRepository:IGenericRepository<ClientOrders>
    {
        Task<IEnumerable<ClientOrders>> GetRandom4Orders();
    }
}
