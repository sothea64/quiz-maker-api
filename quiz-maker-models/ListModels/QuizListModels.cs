using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.ListModels
{
    public class BasicListModel
    {
        public int Id { get; set; }
    }

    public class QuizListModel : BasicListModel
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
