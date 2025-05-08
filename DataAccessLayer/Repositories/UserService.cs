

using BL.IRepositories;
using DataAccessLayer.Data;

namespace DataAccessLayer.Repositories
{
    public class UserService : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserService(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return dbContext.Users.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }
    }
}
