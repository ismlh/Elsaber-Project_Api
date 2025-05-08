
namespace BL.IRepositories
{
   public interface ISecurityRepository
    {
        Task<isHasToken> Login(LoginDto loginDto);
    }
}
