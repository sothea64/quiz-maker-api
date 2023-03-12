using quiz_maker_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.OutfaceModels
{
    public class LoginOutfaceModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RefreshTokenOutfaceModel
    {
        public string RefreshToken { get; set; }
        public string AuthorizeToken { get; set; }
    }

    public class AuthorizedUserModel
    {
        public string AuthorizeToken { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpireTime { get; set; }
        public User User { get; set; }
    } 
}
