/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Proto
{
    public class GameSession : Proto.ISerializerable
    {
        public GameSession()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public int SessionID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int UserID { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            SessionID = reader.ReadInt32();
            UserID = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(SessionID);
            writer.Write(UserID);
            
        }

    }
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
    public class S2C_Login : Proto.ISerializerable
    {
        public S2C_Login()
        {
            SessionKey = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public bool Success { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string SessionKey { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Success = reader.ReadBoolean();
            SessionKey = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Success);
            var SessionKey_bytes = Encoding.UTF8.GetBytes(SessionKey);writer.Write(SessionKey_bytes.Length);writer.Write(SessionKey_bytes);
            
        }

    }
    public class C2S_SaveData : Proto.ISerializerable
    {
        public C2S_SaveData()
        {
Session = new GameSession();
            Json = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public GameSession Session { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Json { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Session = new GameSession();Session.ParseFormBinary(reader);
            Json = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            Session.ToBinary(writer);
            var Json_bytes = Encoding.UTF8.GetBytes(Json);writer.Write(Json_bytes.Length);writer.Write(Json_bytes);
            
        }

    }
    public class S2C_SaveData : Proto.ISerializerable
    {
        public S2C_SaveData()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public bool Success { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Success = reader.ReadBoolean();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Success);
            
        }

    }
}