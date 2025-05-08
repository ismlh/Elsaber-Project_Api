
using BL.IRepositories;
using DataAccessLayer.Data;

namespace DataAccessLayer.Repositories
{
    public class CatrgoryService : GenericRepository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CatrgoryService(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public  async Task<IEnumerable<Category>> CategoriesWithProductsAsync()
        {
            return await _context.Categories.Include(x => x.Products).ToListAsync();
        }

        public async Task<Category> CategoryWithProductsAsync(int id)
        {
            var cateegory=await _context.Categories.Include(x=>x.Products).ThenInclude(p=>p.Images).FirstOrDefaultAsync(x=>x.Id==id);
            return cateegory;
        }
    }
}
