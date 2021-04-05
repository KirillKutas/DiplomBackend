using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DbProject.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CryptoController : ControllerBase
    {
        delegate void SendPrice(object obj);
        private readonly IHubContext<WSHub> _hubContext;
        public CryptoController(IHubContext<WSHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet]
        [Route("GetPrice")]
        public void GetPrice()
        {
            SendPrice sendPrice = async (object obj) =>
            {
                var message = ResponseCreator.Create(new
                {
                    y = DateTime.Now.Millisecond,
                    x = DateTime.Now.Second % 5 == 0 ? DateTime.Now.Second.ToString() : ""
                });

               await _hubContext.Clients.All.SendAsync("Send", message);
            };
            TimerCallback tm = new TimerCallback(sendPrice);
            Timer timer = new Timer(tm, null, 0, 1000);

        }
    }
}
