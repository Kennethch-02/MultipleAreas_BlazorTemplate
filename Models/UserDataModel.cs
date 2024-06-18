namespace MultipleAreas_BlazorTemplate.Models
{
    public class UserDataModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ErrorInLogin { get; set; }
        public bool changePassword { get; set; }
        public bool LoginFail { get; set;} = false;
    }
}
