

namespace BL.IUnitOfWork
{
  public  interface IUnitOfWork :IDisposable
    {
        ICompanyRepository Companies { get; }
        ICategoryRepository Categories { get; }

        IServicesRepository Services { get; }

        IProductRepository Products { get; }
        IIMageRepository Images { get; }

        IHomeRepository Home { get; }
        IUserRepository Users { get; }
        ClientOrderRepository ClientOrders { get; }

        IQuestionRepository Questions { get; }
        public int Complete();
    }
}
