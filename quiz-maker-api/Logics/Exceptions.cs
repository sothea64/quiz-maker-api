using quiz_maker_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.Logics
{
    public class LogicException : Exception
    {
        public string Code { get; set; }
        protected string _message;
        public object Model { get; set; }

        public LogicException()
        {

        }

        public LogicException(string message) : base(message)
        {

        }
    }

    public class DuplicateException : Exception
    {
        private string message { get; set; } = string.Empty;
        public override string Message => message;
        public DuplicateException (string objectName)
        {
            message = string.Format(ErrorRexs._MsgDuplicateObject_Coloumns, objectName);
        }
    }

    public class InUsedException : Exception
    {
        private string message { get; set; } = string.Empty;
        public override string Message => base.Message;
        public InUsedException(string objectName)
        {
            message = string.Format(ErrorRexs._MsgInusedData, objectName);
        }
    }

    public class UnAuthorizeException : Exception
    {
        public UnAuthorizeException(string message) : base(message)
        {

        }
    }
}
