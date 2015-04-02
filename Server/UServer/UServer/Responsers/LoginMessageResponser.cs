using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UServer.Responsers
{
    [Responser(typeof(PNet.LoginMessage))]
    class LoginMessageResponser : ResponseHandler
    {
        public override void Handle()
        {
            var message = this.Message as PNet.LoginMessage;
            var request = message.Request;
            if (request.Token != null)
            {
                var response = new Proto.S2C_Login();
                response.SessionKey = DateTime.Now.Ticks.ToString();
                response.Success = true;
                response.Session = new Proto.GameSession
                {
                    SessionID = DateTime.Now.Millisecond,
                    UserID = 1
                };
                message.SetResponse(response) ;
            }
            //end
        }
    }
}
