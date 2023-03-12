using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.Models
{
    public class BaseModel : ICloneable
    {
        public int Id { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
