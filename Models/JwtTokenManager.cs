using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace Demoproject.Models
{
    public class JwtTokenManager : IwjtTokenManager
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContaxt appDbContaxt;

        public JwtTokenManager(IConfiguration configuration,AppDbContaxt appDbContaxt)
        {
            this._configuration = configuration;
            this.appDbContaxt = appDbContaxt;
        }
        public string Authenticate(string Email, string Password)
        {
            var data=appDbContaxt.login.Where(e=>e.Email==Email).FirstOrDefault();
            if (data != null)
            {
                bool isValid = (data.Email == Email && decryptPassword(data.Password) == Password);
                if (isValid)
                {
                    var Key = _configuration.GetValue<string>("JwtConfig:Key");
                    var KeyBytes = Encoding.ASCII.GetBytes(Key);

                    var tokenHandler = new JwtSecurityTokenHandler();

                    var tokenDescription = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {

                    new Claim(ClaimTypes.NameIdentifier, Email),
                     new Claim(ClaimTypes.Role,data.roles)
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(3),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(KeyBytes), SecurityAlgorithms.HmacSha256)

                    };
                    var token = tokenHandler.CreateToken(tokenDescription);
                    return tokenHandler.WriteToken(token);

                }
                else
                {
                    return "Invalid Password";
                }

            }
            else
                return "Email Not Found";

           

        }
        public static string decryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] encryptpassword =Convert.FromBase64String(password);
                string decryptedPassword=ASCIIEncoding.ASCII.GetString(encryptpassword);
                return decryptedPassword;
            }
        }
    }
}
