using quiz_maker_models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string HashPasswrd { get; set; }
        public UserRole UserRole { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
