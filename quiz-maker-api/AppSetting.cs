using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api
{
    public class AppSetting
    {
      public string ConnectionString { get; set; } = "server=SOTHEA-PC\\SQLEXPRESS;uid=sothea;pwd=underadmin;database=QuizMaker_DB";

        private static SymmetricSecurityKey _secret_key;
        public static SymmetricSecurityKey SECRET_KEY
        {
            get
            {
                if (_secret_key == null)
                {
                    _secret_key = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());
                }
                return _secret_key;
            }
        }
    }
}
