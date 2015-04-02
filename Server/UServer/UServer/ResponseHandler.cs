using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UServer
{
    public abstract class ResponseHandler
    {
        public XNet.Libs.Net.Client Client { set; get; }

        public PNet.INetMessage Message { set; get; }

        public abstract void Handle();
    }

    public class ResponserAttribute : Attribute
    {
        public ResponserAttribute(Type mType)
        {
            MessageType = mType;
        }

        public Type MessageType { set; get; }
    }
}
