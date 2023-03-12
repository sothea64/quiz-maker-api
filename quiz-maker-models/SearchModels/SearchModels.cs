using quiz_maker_models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.SearchModels
{
    public class QuizSearchModel : BasicSearchModel
    {
        public bool ShowDeactivateQuiz { get; set; } = false;
    }

    public class UserSearchModel : BasicSearchModel
    {
        public UserRole? Role { get; set; }
        public bool ShowDeactivateUser { get; set; } = false;
    }
}
