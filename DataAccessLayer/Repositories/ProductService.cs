
using BL.Dtos;
using BL.IRepositories;
using DataAccessLayer.Data;

namespace DataAccessLayer.Repositories
{
    public class ProductService : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductService(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoriesAndImages()
        {
            return await this.dbContext.Products.Include(x => x.Category).Include(x=>x.Images).ToListAsync();
        }

        public async Task<Product> GetByIDWithCategoriesandImages(int id)
        {
            return await this.dbContext.Products.Include(x => x.Category).Include(x=>x.Images).FirstOrDefaultAsync(x=>x.Id==id);

        }

        public async Task<IEnumerable<Product>> GetPaginetedData(int pageNumber, int pageSize,int categoryId=0)
        {
            var query = await this.dbContext.Products.Include(p => p.Category).Include(p => p.Images).ToListAsync();
            if (categoryId > 0)
            {
                var category = await dbContext.Categories.FindAsync(categoryId);
                if (category != null)
                    query = query.Where(p => p.CategoryId == categoryId).ToList();
            }
            return  query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }


        public async Task<ProductWithImagesDto> ProductWithImage(int id)
        {
            var product= await this.dbContext.Products.Include(x => x.Images).FirstOrDefaultAsync(x=>x.Id==id);
            if (product != null)
            { 
                return new ProductWithImagesDto
                { Name = product.Name, Images = product.Images.Select(p => p.Image).ToList() };
            }
            return null;
        }
    }
}
