/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Proto
{
    public class C2S_Login : Proto.ISerializerable
    {
        public C2S_Login()
        {
            Token = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public string Token { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public UserType Type { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Token = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Type = (UserType)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var Token_bytes = Encoding.UTF8.GetBytes(Token);writer.Write(Token_bytes.Length);writer.Write(Token_bytes);
            writer.Write((int)Type);
            
        }

    }
}