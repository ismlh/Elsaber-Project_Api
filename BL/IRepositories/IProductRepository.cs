
using BL.Dtos;

namespace BL.IRepositories
{
  public  interface IProductRepository:IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoriesAndImages();
        Task<Product> GetByIDWithCategoriesandImages(int id);
        Task<ProductWithImagesDto> ProductWithImage(int id);

        Task<IEnumerable<Product>> GetPaginetedData(int pageNumber, int pageSize,int categoryId=0);
    }
}
