

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BL.Dtos;
using BL.IRepositories;
using BL.Models;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DataAccessLayer.Repositories
{
    public class UserManagerService : ISecurityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration configuration;


        public UserManagerService()
        {
            
        }
        public UserManagerService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this.configuration = configuration;
        }
        public async Task<isHasToken> Login(LoginDto loginModel)
        {

            var User = await _userManager.FindByNameAsync(loginModel.Username);
            if (User != null)
            {
                var result = await _userManager.CheckPasswordAsync(User, loginModel.Password);
                if (result)
                {

                    #region Claims

                    // User Claims ==> In Body
                    var userCliams = new List<Claim>();
                    // add Guid To Change Token Every TIme He Login
                    userCliams.Add(new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()));
                    userCliams.Add(new Claim(ClaimTypes.NameIdentifier, User.Id));
                    userCliams.Add(new Claim(ClaimTypes.Name, User.UserName));

                    // get Roles To Add To This User
                    var userRoles = await _userManager.GetRolesAsync(User);
                    foreach (var role in userRoles)
                        userCliams.Add(new Claim(ClaimTypes.Role, role));
                    #endregion


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

                    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                    // design My Token
                    var myToken = new JwtSecurityToken
                        (
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            claims: userCliams,
                            signingCredentials: signingCredentials
                        );
                    // Now We Need To return Token
                    return new isHasToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(myToken),
                        IsToken = true,
                        ExpiredDate= DateTime.Now.AddHours(2)
                    };
                }
            }
            
                  return new isHasToken()
                  {
                      Token = ("اسم المستخدم او كلمة السر خاطئه "),
                      IsToken = false,
                      ExpiredDate = DateTime.Now.AddHours(2)
                  };
           
        }

        

    }
}


