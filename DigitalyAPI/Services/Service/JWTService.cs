using DigitalyAPI.Models.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DigitalyAPI.Services.Service
{
    public class JWTService 
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtkey;

        // create a constructor and ineject configuration in this class ( _jwtkey provided in apprentissage.json )
        public JWTService(IConfiguration config)
        {
            _config = config;
            // _jwtkey is used for both encrypting and decripting the jwt token 
            _jwtkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));

        }

        public string createJWT(User user)
        {
            var userclaims = new List<Claim>
             {
            new Claim (ClaimTypes.NameIdentifier, user.Id),
            new Claim (ClaimTypes.Email, user.Email),
            new Claim (ClaimTypes.GivenName, user.FirstName),
            new Claim (ClaimTypes.Surname, user.LastName),
            // remove after 
              new Claim ("this is my own claim", "this is the value "),

         //   new Claim("roles", string.Join(",", user.Roles)) // Si les rôles sont une liste

              };

       

            // from this import : using System.IdentityModel.Tokens.Jwt;

            var credentials = new SigningCredentials(_jwtkey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userclaims),
                Expires = DateTime.UtcNow.AddMinutes(100),
                Issuer = _config["JWT:Issuer"],  
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);


        }
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        public SymmetricSecurityKey GetSecurityKey()
        {
            return _jwtkey; // This is the private field already initialized in the constructor
        }



    }




}
