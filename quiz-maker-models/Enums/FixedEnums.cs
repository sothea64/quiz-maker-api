using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.Enums
{
    public enum QuestionTypes
    {
        Text = 1,
        MultipleChoices,
        CheckedBox
    }

    public enum UserRole
    {
        Admin = 1,
        DepartmentManager,
        Examinee
    }
}
