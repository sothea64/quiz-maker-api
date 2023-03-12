using Microsoft.AspNetCore.Http;
using quiz_maker_models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace quiz_maker_api.Helpers
{
    public class HttpCurrentScope : ICurrentScope
    {
        readonly HttpContext _context;
        public HttpCurrentScope(IHttpContextAccessor httpContextAccessor, QuizMakerDbContext db)
            : base(db)
        {
            _context = httpContextAccessor.HttpContext;
        }

        public HttpCurrentScope(HttpContext httpContext, QuizMakerDbContext db)
             : base(db)
        {
            _context = httpContext;
        }

        HttpContext HttpContext => _context;


        public override int GetUserId()
        {
            var uid = Parse.ToInt(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value);
            return uid;
        }

        public override string GetUserName()
        {
            var uid = Parse.ToString(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value);
            return uid;
        }

        public override ClientInfo GetClientInfo()
        {
            var clientInfo = new ClientInfo();
            clientInfo.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            clientInfo.HostName = HttpContext.Request.Headers["X-Client-HostName"];
            clientInfo.MacAddress = HttpContext.Request.Headers["X-Client-MacAddress"];
            clientInfo.Version = HttpContext.Request.Headers["X-Client-Version"];
            clientInfo.OSVersion = HttpContext.Request.Headers["X-Client-OSVersion"];
            if (HttpContext.Request.Headers.Keys.Contains("X-Client-Time"))
            {
                clientInfo.Time = Parse.ToDateTime(HttpContext.Request.Headers["X-Client-Time"]);
            }
            return clientInfo;
        }
    }
}
