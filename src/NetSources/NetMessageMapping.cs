using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Proto;

namespace PNet
{

    [NetMessage]
    public class LoginMessage : NetMessage<C2S_Login, S2C_Login> { }

    [NetMessage]
    public class RegUserMessage : NetMessage<C2S_RegUser, S2C_RegUser> { }

}