using BL.IRepositories;
using DataAccessLayer.Data;


namespace DataAccessLayer.Repositories
{
    public class CompanyService : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }    
    }
}
