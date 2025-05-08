

using BL.IRepositories;
using DataAccessLayer.Data;

namespace DataAccessLayer.Repositories
{
    public class ServicesRepository : GenericRepository<Services>, IServicesRepository
    {
        public ServicesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
