using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MultipleAreas_BlazorTemplate.Models;

namespace MultipleAreas_BlazorTemplate.Services.Auth
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDataModel?> LoginAsync(UserDataModel userDataModel)
        {
            userDataModel = bdLogin(userDataModel);
            if (userDataModel.LoginFail) return userDataModel;
            string userDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(userDataModel);
            try {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userDataModel.UserName),
                    new Claim(ClaimTypes.Role, userDataModel.Role),
                    new Claim(ClaimTypes.UserData, userDataJson)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(GlobalConfigModel.configuration.GetValue<int>("UserSessionExpires"))
                };

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties);
            }
            catch (System.Exception e)
            {
                return null;
            }
            return userDataModel;
        }
        private UserDataModel bdLogin (UserDataModel userDataModel)
        {
            // Implementar lógica de acceso a la base de datos
            if (userDataModel.Password != "test") {
                userDataModel.ErrorInLogin = "User or password incorrect";
                userDataModel.LoginFail = true;
            }
            else if (userDataModel.UserName == "admin")
            {
                userDataModel.Role = "Admin";
            }
            else if (userDataModel.UserName == "user")
            {
                userDataModel.Role = "User";
            }
            else if (userDataModel.UserName == "root")
            {
                userDataModel.Role = "Root";
            }
            else if (userDataModel.UserName == "dev")
            {
                userDataModel.Role = "Dev";
            }
            else {
                userDataModel.ErrorInLogin = "User or password incorrect";
                userDataModel.LoginFail = true;
            }
            return userDataModel;
        }
        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
