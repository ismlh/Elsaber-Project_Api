
using BL.IRepositories;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataAccessLayer.Repositories
{
    public class ClientOrderService : GenericRepository<ClientOrders>, ClientOrderRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ClientOrderService(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ClientOrders>> GetRandom4Orders()
        {
            var query =await dbContext.ClientOrders
                            .OrderBy(x => Guid.NewGuid())
                            .ToListAsync();
            if(query.Count>4)
                return query.Take(4);
            return query;
        }
    }
}
