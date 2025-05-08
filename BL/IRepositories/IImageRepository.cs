
namespace BL.IRepositories
{
 public   interface IIMageRepository:IGenericRepository<Images>
    {
        Task<Images> GetImageWithProduct(int id);
    }
}
