



using BL.Dtos;
using BL.IRepositories;
using DataAccessLayer.Data;

namespace DataAccessLayer.Repositories
{
    public class HomeService : IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        public HomeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CompanyDtoToRead> GetCompanyData()
        {
            var company = await _context.Companies.FirstOrDefaultAsync();
            if (company == null) throw new Exception("Company Not Found");
            return new CompanyDtoToRead
            {
                Name = company.Name,
                Title = company.Title,
                Phone = company.Phone,
                Description = company.Description,
                LogoUrl = company.LogoUrl,
                YoutubeUrl = company.YoutubeUrl,
                FacebookUrl = company.FacebookUrl,
                InstgramUrl = company.InstgramUrl,
                TwitterUrl = company.TwitterUrl
            };
        }

        public async Task<IEnumerable<ServiceDtoToRead>> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            return services.Select(s => new ServiceDtoToRead
            {
                Name = s.Name,
                Description = s.Description,
                ImageUrl = s.ImageUrl
            }).ToList();
        }
    }
}
