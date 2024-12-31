using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DigitalyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jwtservice;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(JWTService jwtservice , 
            SignInManager<User> signInManager , 
            RoleManager<IdentityRole> roleManager ,
            UserManager<User> userManager ,
            EmailService emailService ,
            IConfiguration config)
        {
            _jwtservice = jwtservice;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
            _roleManager= roleManager;
        }

        [Authorize]
        /* [HttpGet("refresh-token")]

         public async Task <ActionResult<UserDto>> RefreshUsertoken ()
         {
             var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
             return CreateApplicationUserDto(user);
         }
        */
        [Authorize]
        [HttpGet("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshUsertoken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            if (user == null) return Unauthorized("User not found.");

            // Retrieve roles assigned to the user
            var userRoles = await _userManager.GetRolesAsync(user);

            // Pass the user and their roles to CreateApplicationUserDto
            return CreateApplicationUserDto(user, userRoles);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken(string accessToken, string refreshToken)
        {
            // 1. Validate the access token
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(accessToken))
            {
                return Unauthorized("Invalid access token.");
            }

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtservice.GetSecurityKey(), // Retrieve the signing key
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = false, // Not validating audience in this example
                ValidateLifetime = false, // Ignore expiration (we're only using this to extract user info)
                ClockSkew = TimeSpan.Zero
            };

            ClaimsPrincipal principal;
            try
            {
                principal = jwtHandler.ValidateToken(accessToken, tokenValidationParams, out var validatedToken);
                if (validatedToken is not JwtSecurityToken jwt || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Unauthorized("Invalid access token.");
                }
            }
            catch
            {
                return Unauthorized("Invalid access token.");
            }

            // 2. Extract user information from the access token
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid access token.");
            }

            // 3. Validate the refresh token
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId && u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            // 4. Generate new tokens
            var newJwt = _jwtservice.createJWT(user); // Create a new access token
            var newRefreshToken = _jwtservice.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1); // Refresh token valid for 7 days
            await _userManager.UpdateAsync(user);

            // 5. Retrieve roles assigned to the user
            var userRoles = await _userManager.GetRolesAsync(user);

            // 6. Return new tokens
            return Ok(new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = newJwt,
                Roles = userRoles.ToList(),
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);
            }

            return Ok(new { message = "Logged out successfully." });
        }



        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            // Recherche de l'utilisateur par email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("Invalid Email or password.");
            if (!user.EmailConfirmed) return Unauthorized("Please confirm your email.");

            // Vérification du mot de passe
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid Email or password.");

            // Générer un nouveau refresh token
            var refreshToken = _jwtservice.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Durée de validité 7 jours

            // Sauvegarder le refresh token dans la base de données
            await _userManager.UpdateAsync(user);

            // Récupérer les rôles associés à l'utilisateur
            var userRoles = await _userManager.GetRolesAsync(user);

            // Créer et retourner les données utilisateur avec les tokens
            var userDto = CreateApplicationUserDto(user, userRoles);
            userDto.UserId = user.Id; // Add the UserId here
            userDto.RefreshToken = refreshToken;

            return Ok(userDto);
        }




        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model, string role)
        {
            if (await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest($"An existing account is using {model.Email} as the email address. Please try with another email.");
            }

            var userToAdd = new User
            {
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                Email = model.Email.ToLower(),
                UserName = model.Email.ToLower(),  // UserName is also set to Email

                PhoneNumber = model.PhoneNumber,
                Fax = model.Fax,
                Civilite = model.Civilite,
                Dateofbirth = model.Dateofbirth,
                Address = model.Address,
                City =model.City,
                PostalCode =model.PostalCode,
                Country =model.Country,
                Grade =model.Grade,
                longInPosition =model.longInPosition,
                PhoneFix =model.PhoneFix,

    };

            // Create the user in the Identity database
            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            // Ensure the specified role exists
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest("This role doesn't exist. Please try another role.");
            }

            // Assign role to the user
            var roleResult = await _userManager.AddToRoleAsync(userToAdd, role);
            if (!roleResult.Succeeded)
            {
                return BadRequest("Failed to assign role to user.");
            }

            // Send email confirmation link if role assignment succeeds
            try
            {
                if (await SendConfirmEmailAsync(userToAdd))
                {
                    return Ok(new { title = "Account Created", message = "Please confirm your email address." });
                }
                return BadRequest("Failed to send email. Please contact the administrator.");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email. Please contact the administrator.");
            }
        }




        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("This email address has not been registered yet.");

            if (user.EmailConfirmed== true)
                return BadRequest("Your email was already confirmed. Please log in to your account.");

            try
            {
                // Décoder le jeton pour la confirmation
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                // Tenter la confirmation de l'email
                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

                if (result.Succeeded)
                {
                    return Ok(new { title = "Email Confirmed", message = "Your email address is confirmed. You can log in now." });
                }
                else
                {
                    // Si la confirmation échoue, renvoyer les erreurs
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(new { title = "Invalid Token", errors });
                }
            }
            catch (Exception)
            {
                // Retourne une erreur en cas de problème avec le décodage
                return BadRequest("Invalid token. Please try again.");
            }
        }


        [HttpPost("resend-email-confirmation-link/{email}")]
        public async Task <IActionResult> ResendEmailConfirmationLink (string email)
        {

            if ( string.IsNullOrEmpty(email) ) return BadRequest("Invalid email "); 
            var user = await _userManager.FindByEmailAsync (email);

            if (user == null) return Unauthorized("this email adress  has not been registed yet ");
            if (user.EmailConfirmed == true) return BadRequest("Your email has been confirmed before . Please login to your account ");

            try
            {
                if ( await SendConfirmEmailAsync(user) )
                {
                    return Ok(new { title = "Confirmation link resent ", message = "Please Confirm your email adress ." });



                }
                return BadRequest("Failed to resend Email Confirmation Link  ");


            }
            catch(Exception)
            {
                return BadRequest(" Exception : Failed to resend Email Confirmation Link  ");

            }
        }

        [HttpPost("forgot-username-or-password/{email}")]
        public async Task<IActionResult> ForgotUsernameOrPassword (string email)
        {
            if ( string.IsNullOrEmpty(email) ) { return BadRequest("Invalid email"); }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Unauthorized("this email adress  has not been registed yet ");
            if (user.EmailConfirmed == false) return BadRequest("Please Confirm your email adress first  ");

            try
            {
                if ( await SendForgotUsernameOrPasswordEmail(user))
                {
                    return Ok(new JsonResult(new { title = "Forgot username or password email sent ", message = " Please Check your email" }));


                }
                return BadRequest(" Failed to send Emailfor usrname or password . Please contact your ADMIN .  ");


            }
            catch (Exception)
            {
                return BadRequest(" Exception :Failed to send Emailfor usrname or password . Please contact your ADMIN .  ");


            }

        }

        [HttpPut("reset-password")]

        public async Task<IActionResult> ResetPassword (ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return Unauthorized("this email adress  has not been registed yet ");
            if (user.EmailConfirmed == false) return BadRequest("Please Confirm your email adress first  ");

            try
            {
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

                if (result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = " Password reset successfuly", message = " Your Password has been reset " }));
                }

                return BadRequest(" Invalid token .Please try again ");

            }
            
            
            
            catch(Exception)
            {
                return BadRequest(" Exception :Invalid token .Please try again ");

            }
        }



        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Mise à jour des informations utilisateur
            user.FirstName = updateDto.FirstName ?? user.FirstName;
            user.LastName = updateDto.LastName ?? user.LastName;
            user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
            user.Fax = updateDto.Fax ?? user.Fax;
            user.Civilite = updateDto.Civilite ?? user.Civilite;
            user.Address = updateDto.Address ?? user.Address;
            user.City = updateDto.City ?? user.City;
            user.PostalCode = updateDto.PostalCode ?? user.PostalCode;
            user.Country = updateDto.Country ?? user.Country;
            user.Grade = updateDto.Grade ?? user.Grade;
            user.longInPosition = updateDto.LongInPosition ?? user.longInPosition;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update profile.");
            }

            return Ok("Profile updated successfully.");
        }

        #region private Helper Methods
        private UserDto CreateApplicationUserDto(User user, IList<string> roles)
{
    return new UserDto
    {
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,  
        Roles = roles.ToList(),
        JWT = _jwtservice.createJWT(user),
        UserId = user.Id, // Populate UserId

        PhoneNumber = user.PhoneNumber,  
        PhoneFix = user.PhoneFix,  
        Fax = user.Fax,  
        Civilite = user.Civilite,  
        Dateofbirth = user.Dateofbirth,  
        Address = user.Address,  
        City = user.City,  
        PostalCode = user.PostalCode,  
        Country = user.Country,  
        Grade = user.Grade,  
        longInPosition = user.longInPosition, 
    };
}


        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x=>x.Email == email.ToLower());

        }

        private async Task<bool> SendConfirmEmailAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JWT:ClientUrl"]}/ {_config["Email:ConfirmEmailPath"]}? token={token}&email={user.Email}";

            var body = $"<p> Hello : {user.FirstName} {user.LastName} </p>" +
                "<p> Please Confirm your email adress by clicking the following link </p>" +
                $"<p> <a href=\"{url}\">click here </a> </p>" +
                "<p> Thank You ! </p>" +
                $"<br>{_config["Email:ApplicationName"]} ";

            var emailSend = new EmailSendDto(user.Email, "confirm your email", body);

            return await _emailService.SendEmailAsync(emailSend);   



        }

        private async Task<bool> SendForgotUsernameOrPasswordEmail (User user)
        {

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JWT:ClientUrl"]}/ {_config["Email:RsestPasswordPath"]}? token={token}&email={user.Email}";

            var body = $"<p> Hello : {user.FirstName} {user.LastName} </p>" +
               $"<p>  Username:   {user.UserName}  .</p>" +
                "<p> In order to reset your password . Please Click on the following link   </p>" +
               $"<p> <a href=\"{url}\">click here </a> </p>" +
               "<p> Thank You ! </p>" +
               $"<br>{_config["Email:ApplicationName"]} ";

            var emailSend = new EmailSendDto(user.Email, "Forgot username or password ", body);

            return await _emailService.SendEmailAsync(emailSend);
        }

        #endregion




    }

}