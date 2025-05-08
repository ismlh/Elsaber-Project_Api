

using BL.IRepositories;
using BL.IUnitOfWork;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public ICompanyRepository Companies { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public IServicesRepository Services { get; private set; }

        public IProductRepository Products { get; private set; }
        public IIMageRepository Images { get; private set; }
        public IHomeRepository Home { get; private set; }
        public IUserRepository Users { get; private set; }

        public ClientOrderRepository ClientOrders {  get; private set; }

        public IQuestionRepository Questions { get; private set; }


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            this.Companies = new CompanyService(_dbContext);
            this.Categories=new CatrgoryService(_dbContext);
            this.Services = new ServicesRepository(_dbContext);
            this.Products = new ProductService(_dbContext);
            this.Images = new ImagesService(_dbContext);
            this.Users = new UserService(_dbContext);
            this.Home = new HomeService(_dbContext);
            this.ClientOrders = new ClientOrderService(_dbContext);
            this.Questions = new QuestionService(_dbContext);
        }
        public int Complete()
        {
           return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
           _dbContext.Dispose();
        }
    }
}
