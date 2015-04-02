using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UServer
{
    public class ProtoTool
    {
        public static T Deserialize<T>(byte[] bytes) where T : Proto.ISerializerable, new ()
        {
            var newp = new  T();
            using (var mem = new MemoryStream(bytes))
            {
                using (var br = new BinaryReader(mem))
                {
                    newp.ParseFormBinary(br);
                }

            }
            return newp;
        }

        public static byte[] Serialize<T>(T t) where T : Proto.ISerializerable
        {
            byte[] bytes;
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    t.ToBinary(bw);
                }
                bytes = mem.ToArray();
            }
            return bytes;
        }
    }
}
