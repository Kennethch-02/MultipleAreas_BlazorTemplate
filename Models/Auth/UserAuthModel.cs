namespace MultipleAreas_BlazorTemplate.Models.Auth
{
    public class UserAuthModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
