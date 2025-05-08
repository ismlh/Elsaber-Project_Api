

namespace BL.IRepositories
{
  public  interface IHomeRepository
    {
       Task<CompanyDtoToRead> GetCompanyData();
       Task<IEnumerable<ServiceDtoToRead>> GetServices();
    }
}
