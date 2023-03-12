using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.Models
{
    public class Company : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }

    public class Department : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int CompanyId { get; set; }
    }
}
