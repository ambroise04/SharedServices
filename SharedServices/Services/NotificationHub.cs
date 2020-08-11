using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SharedServices.DAL;
using System;
using System.Threading.Tasks;

namespace SharedServices.UI.Services
{
    public class NotificationHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public NotificationHub(IUserConnectionManager userConnectionManager, 
                               UserManager<ApplicationUser> userManager)
        {
            _userConnectionManager = userConnectionManager;
            _userManager = userManager;
        }
        public string GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();
            //var userId = httpContext.Request.Query["userId"];
            var userId = _userManager.GetUserId(httpContext.User);
            _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

            return Context.ConnectionId;
        }
        //Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
    }
}