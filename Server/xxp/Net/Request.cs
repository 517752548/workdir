using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XNet.Libs.Net
{
    /// <summary>
    /// 服务器响应处理结果
    /// @author：xxp
    /// @date：2013/01/10
    /// </summary>
    public class ResponserResult : ASerializer.ISerializerable
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { set; get; }
        /// <summary>
        /// 成功的结果
        /// </summary>
        public string Result { set; get; }
        /// <summary>
        /// 请求ID
        /// </summary>
        public long RequesterID { set; get; }
        /// <summary>
        /// 请求API
        /// </summary>
        public string API { get; set; }

        #region ISerializerable 成员

        public void ParseFormBinary(byte[] bytes)
        {
            using (var mem = new MemoryStream(bytes))
            {
                using (var br = new BinaryReader(mem))
                {
                    RequesterID = br.ReadInt64();
                    Success = br.ReadBoolean();
                    API = Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32()));
                    Result = Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32()));
                }
            }
        }

        public byte[] ToBinary()
        {
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    var api = Encoding.UTF8.GetBytes(API ?? string.Empty);
                    var result = Encoding.UTF8.GetBytes(Result ?? string.Empty);
                    bw.Write(RequesterID);
                    bw.Write(Success);
                    bw.Write(api.Length);
                    bw.Write(api);
                    bw.Write(result.Length);
                    bw.Write(result);
                }
                return mem.ToArray();
            }
        }

        #endregion
    }

    /// <summary>
    /// 请求
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public class Request : Net.Libs.ASerializer.ISerializerable
    {
		/// <summary>
		/// for server send response
		/// </summary>
		/// <value>The state of the user.</value>
		public object UserState{ set; get; }
        /// <summary>
        /// API
        /// </summary>
        public string API { set; get; }
        /// <summary>
        /// 请求参数列表
        /// </summary>
        public List<Paramater> Paramaters { set; get; }
        /// <summary>
        /// 请求ID
        /// </summary>
        public long RequesterID { set; get; }
        /// <summary>
        /// 根据 Key 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                string value = string.Empty;
                var par = Paramaters.Where(t => string.Compare(t.Name,key, true)==0 ).FirstOrDefault();
                if (par != null)
                {
                    return par.Value;
                }
                return value;
            }
        }

		public string GetValue(string key)
		{
			return this [key];
		}
        public Request(long id)
            : this()
        {

            RequesterID = id;
        }
        public Request()
        {
            Paramaters = new List<Paramater>();
        }
        /// <summary>
        /// 添加一个参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Request Add(string key, string value)
        {
            Paramaters.Add(new Paramater { Name = key, Value = value });
            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var i in Paramaters)
            {
                sb.Append(string.Format("(Key:{0},Name:{1})", i.Name, i.Value));
            }
            return sb.ToString();

        }

        #region ISerializerable 成员

        public void ParseFormBinary(byte[] bytes)
        {
            using (var mem = new MemoryStream(bytes))
            {
                using (var br = new BinaryReader(mem))
                {
                    RequesterID = br.ReadInt64();
                    API = Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32()));
                    int length = br.ReadInt32();
                    for (var i = 0; i < length; i++)
                    {
                        var key = Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32()));
                        var value = Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32()));
                        this.Paramaters.Add(new Paramater
                        {
                            Value = value,
                            Name = key
                        });
                    }
                }
            }
        }

        public byte[] ToBinary()
        {
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    var api = Encoding.UTF8.GetBytes(API ?? string.Empty);
                    bw.Write(RequesterID);
                    bw.Write(api.Length);
                    bw.Write(api);
                    bw.Write(this.Paramaters.Count);
                    foreach (var i in Paramaters)
                    {
                        var key = Encoding.UTF8.GetBytes(i.Name ?? string.Empty);
                        var value = Encoding.UTF8.GetBytes(i.Value ?? string.Empty);
                        bw.Write(key.Length);
                        bw.Write(key);
                        bw.Write(value.Length);
                        bw.Write(value);
                    }
                }
                return mem.ToArray();
            }
        }

        #endregion
    }

    /// <summary>
    /// 参数
    /// </summary>
    public class Paramater
    {
        /// <summary>
        /// 参数key
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { set; get; }
    }
}
