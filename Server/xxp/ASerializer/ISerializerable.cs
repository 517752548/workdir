using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNet.Libs.ASerializer
{
    public interface ISerializerable
    {
        void ParseFormBinary(byte[] bytes);
        byte[] ToBinary();
    }
}
