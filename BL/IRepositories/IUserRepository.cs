

namespace BL.IRepositories
{
   public interface IUserRepository:IGenericRepository<User>
    {
        Task<User> GetUserByPhoneNumber(string phoneNumber);
    }
}
