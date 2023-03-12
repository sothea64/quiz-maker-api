using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.SearchModels
{
    public interface ISearchModel
    {
        string Search { get; set; }
    }

    public class BasicSearchModel : ISearchModel
    {
        public string Search { get; set; }
    }
}
