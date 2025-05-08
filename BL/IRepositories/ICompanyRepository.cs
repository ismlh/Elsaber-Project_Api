
using BL.Dtos;
using Microsoft.AspNetCore.Http;

namespace BL.IRepositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        //public Task<byte[]> ConvertFileToArrayOfByteAsync(IFormFile file);
    }
}
