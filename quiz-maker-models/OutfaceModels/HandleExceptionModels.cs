using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.OutfaceModels
{
    public class HandleException
    {
        public string Error { get; set; }
        public string Type { get; set; }
        public string Stack { get; set; }
        public string Code { get; set; }
    }
}
