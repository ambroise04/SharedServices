using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SharedServices.UI.Extensions;

namespace SharedServices.UI.Services
{
    public class SignalRHub : Hub
    {
        private ISession session;

        public SignalRHub(ISession session)
        {
            this.session = session;
        }

        public void LogParams()
        {
            var connectionId = Context.ConnectionId;
            if (session.GetValue<string>("Connection") == default)
            {
                session.InsertValue("Connection", connectionId);
            }
        }
    }
}