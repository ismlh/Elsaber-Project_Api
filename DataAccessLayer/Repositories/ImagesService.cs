
using BL.IRepositories;
using DataAccessLayer.Data;

namespace DataAccessLayer.Repositories
{

    public class ImagesService : GenericRepository<Images>, IIMageRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ImagesService(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Images> GetImageWithProduct(int id)
        {
            return await dbContext.Images.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
