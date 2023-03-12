using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.Helpers
{
    public class ICurrentScope
    {
        QuizMakerDbContext _db;
        public ICurrentScope(QuizMakerDbContext db)
        {
            _db = db;
        }

        ClientInfo _clientInfo = null;
        public ClientInfo ClientInfo
        {
            get
            {
                if (_clientInfo == null)
                {
                    _clientInfo = GetClientInfo();
                }
                return _clientInfo;
            }
        }

        private int _defaultUserId = 0;
        private string _defaultUserName = "";

        public void SetDefault(string userName = "", int userId = 0, int branchId = 0, int companyId = 0)
        {
            _defaultUserId = userId;
            _defaultUserName = userName;
        }

        public int UserId => GetUserId();

        public string UserName => GetUserName();

        public Guid SessionId => GetSessionId();

        public virtual int GetUserId()
        {
            return _defaultUserId;
        }

        public virtual string GetUserName()
        {
            return _defaultUserName;
        }
        public virtual Guid GetSessionId()
        {
            return Guid.Empty;
        }

        public virtual ClientInfo GetClientInfo()
        {
            return new ClientInfo();
        }
    }

    public class ClientInfo
    {
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public string HostName { get; set; }
        public string OSVersion { get; set; }
        public string UserAgent { get; set; }
        public string Version { get; set; }
        public DateTime Time { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
