using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Libs.Utility;

namespace UServer
{
    public class MessageHandler : XNet.Libs.Net.MessageHandler
    {
        public override void Handle(XNet.Libs.Net.Message message, XNet.Libs.Net.Client client)
        {
            using (var mem = new MemoryStream(message.Content))
            {
                using (var br = new BinaryReader(mem))
                {
                    var no = br.ReadInt32();
                    Type handlerMessage;
                    if (!MessageList.TryGetValue(no, out handlerMessage))
                    {
                        XNet.Libs.Utility.Debuger.LogError(string.Format("编号:{0}消息没有定义", no));
                        return;
                    }
                    var iMessage = Activator.CreateInstance(handlerMessage) as PNet.INetMessage;
                    iMessage.SetResponseFromBytes(br.ReadBytes(br.ReadInt32()));
                    Type responser;
                    if (!Responsers.TryGetValue(no, out responser))
                    {
                        XNet.Libs.Utility.Debuger.LogError(string.Format("编号:{0} 没有response", iMessage.GetType()));
                        return;
                    }
                    var handlerResponse = Activator.CreateInstance(responser) as ResponseHandler;
                    handlerResponse.Client = client;
                    handlerResponse.Message = iMessage;
                    try
                    {
                        handlerResponse.Handle();
                    }
                    catch (Exception ex)
                    {
                        Debuger.LogError(ex);
                    }
                }
            }
            //throw new NotImplementedException();
        }


        private static Dictionary<int, Type> Responsers = new Dictionary<int, Type>();
        private static Dictionary<int, Type> MessageList = new Dictionary<int, Type>();

        static MessageHandler()
        {
            var assembly = typeof(PNet.INetMessage).Assembly;
            var types = assembly.GetTypes();

            foreach (var i in types)
            {
                if (i.GetInterface("INetMessage")==null) continue;
                Console.WriteLine("LoadType:" + i.ToString());
                MessageList.Add(i.Name.GetHashCode(), i);
            }

            var handlerAssembly = typeof(ResponseHandler).Assembly;
            foreach(var i in handlerAssembly.GetTypes())
            {
                if (!i.IsSubclassOf(typeof(ResponseHandler))) continue;
                var attris = i.GetCustomAttributes(typeof(ResponserAttribute), false) as ResponserAttribute[];
                if (attris==null || attris.Length == 0) return;
                Responsers.Add(attris[0].MessageType.Name.GetHashCode(), i);
                Console.WriteLine("Load Response:" + i.ToString());
            }

        }
    }
}
