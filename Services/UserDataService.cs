using MultipleAreas_BlazorTemplate.Interfaces;
using MultipleAreas_BlazorTemplate.Models;
using System.Security.Claims;

namespace MultipleAreas_BlazorTemplate.Services
{
    public class UserDataService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDataService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public UserDataModel GetUserData()
        {
            try {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    return null;
                }
                var userDataJson = user.FindFirst(ClaimTypes.UserData)?.Value;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<UserDataModel>(userDataJson);
            }catch (Exception e)
            {
                return null;
            }

        }
    }
}
