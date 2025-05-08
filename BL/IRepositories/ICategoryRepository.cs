

namespace BL.IRepositories
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        Task<IEnumerable< Category>> CategoriesWithProductsAsync();
        Task< Category> CategoryWithProductsAsync(int id);
    }
}
