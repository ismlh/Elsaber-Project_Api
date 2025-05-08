
using BL.IRepositories;
using DataAccessLayer.Data;

namespace DataAccessLayer.Repositories
{
    public class QuestionService : GenericRepository<Question>, IQuestionRepository
    {
        private readonly ApplicationDbContext _context;
        public QuestionService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
