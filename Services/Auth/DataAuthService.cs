using System.Data.SqlClient;
using System.Data;
using MultipleAreas_BlazorTemplate.Models.Auth;
using MultipleAreas_BlazorTemplate.Data;

namespace MultipleAreas_BlazorTemplate.Services.Auth
{
    public class DataAuthService
    {
        private DataBaseService _conector;
        public DataAuthService()
        {
            _conector = new DataBaseService();
        }
        private string ValidateUserInfo(string user, string password)
        {
            string details = "";
            List<string> detailsList = new List<string>();
            if (string.IsNullOrEmpty(user)) detailsList.Add("User Empty");
            if (string.IsNullOrEmpty(password)) detailsList.Add("Password Empty");

            details = string.Join(", ", detailsList);
            return details;
        }
        public AuthResponseModel doGISSALogin(string user, string pass)
        {
            AuthResponseModel response = new AuthResponseModel();
            string details = ValidateUserInfo(user, pass);
            if (!string.IsNullOrEmpty(details))
            {
                response.Message = details;
                return response;
            }
            else
            {
                DataTable userInfo = _conector.ExecuteStoredProcedure("GISSA.dbo.Buscar_Usuario_GISSA_SOFT", new List<SqlParameter>()
                 {
                    new SqlParameter("@pUsuario",  user)
                 },"GISSA");
                if (userInfo == null)
                {
                    response.Message = "User not found";
                    response.IsAuthenticated = false;
                }
                else if (userInfo.Rows.Count == 0)
                {
                    response.Message = "User not found";
                    response.IsAuthenticated = false;
                }
                else if (userInfo.Rows.Count > 1)
                {
                    response.Message = "Multiple users found";
                    response.IsAuthenticated = false;
                }

                else if (userInfo.Rows.Count == 1)
                {
                    string password = userInfo.Rows[0]["fClave"].ToString().Trim();
                    if (password != null && password.Equals(pass))
                    {
                        response.Message = "Login success";
                        response.IsAuthenticated = false;
                    }
                    else
                    {
                        response.Message = "Password incorrect";
                        response.IsAuthenticated = false;
                    }
                }
            }

            return response;
        }
    }
}
