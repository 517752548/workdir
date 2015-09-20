using PNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UServer.Responsers
{
    [Responser(typeof(RegUserMessage))]
    class RegUserMessageResponser : ResponseHandler
    {
        public override void Handle()
        {
            var message = Message as RegUserMessage;
            message.Response.Success = true;
            message.Response.UserID = 100;
            message.Response.Users = new List<Proto.GameUser>();
        }
    }
}
