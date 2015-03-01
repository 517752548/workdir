using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Tools

{
    public class Utility
    {
        /// <summary>
        /// 获取Streaming Asset 路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetStreamingAssetByPath(string path)
        {
            return Application.streamingAssetsPath + path;
        }
        /// <summary>
        /// 根据一个路径读取文本信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadAStringFile(TextFile file)
        {
            var filePath = file.FileName;
            //if android
            if (filePath.Contains("://"))
            {
                WWW www = new WWW(filePath);
                while (!www.isDone)
                {
                    //wait file load completed
                }
                file.Content = www.text;
            }
            else
                file.Content = System.IO.File.ReadAllText(filePath, XmlParser.UTF8);

            return file.Content;
        }
        /// <summary>
        /// 读取二进制
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] ReadBinaryFile(BinaryFile file)
        {
            var filePath = file.FileName;
            //if android
            if (filePath.Contains("://"))
            {
                WWW www = new WWW(filePath);
                while (!www.isDone)
                {
                    //wait file load completed
                }
                file.Content = www.bytes;
            }
            else
                file.Content = System.IO.File.ReadAllBytes(filePath);

            return file.Content;
        }
        /// <summary>
        /// 读取一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadAStringFile(string file)
        {
            var textFile = new TextFile(file);
            ReadAStringFile(textFile);
            return textFile.Content;
        }

        /// <summary>
        /// 此方法只能在编辑器中使用，游戏中不能使用该方法保存文件
        /// 保存一个文件 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xml"></param>
        public static void WriteStringFile(string path, string xml)
        {
#if UNITY_EDITOR
            using (var sr = new System.IO.StreamWriter(path, false, XmlParser.UTF8))
            {
                sr.Write(xml);
            }
#endif
        }
        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void SaveFileToPersistentPath(string path, byte[] data)
        {
            path = Path.Combine(Application.persistentDataPath, path);
            if (!System.IO.File.Exists(path))
            {
                using (var file = System.IO.File.Create(path))
                {
                }
            }
            System.IO.File.WriteAllBytes(path, data);
            Debug.Log(string.Format("SaveTO:{0} Data:{1}", path, data.Length));
        }

        public static void SaveTextFileToPersistentPath(string path, string str, bool append)
        {
            path = Path.Combine(Application.persistentDataPath, path);

            if (!System.IO.File.Exists(path))
            {
                using (var file = System.IO.File.Create(path))
                {

                }
            }

            if (append)
                File.AppendAllText(path, str, XmlParser.UTF8);
            else
                File.WriteAllText(path, str, XmlParser.UTF8);
            Debug.Log(string.Format("SaveTO:{0} Data:{1}", path, str));
        }

        public static string ReadTextFileFromPersistentPath(string path)
        {
            var bytes = ReadFileFromPersistentPath(path);
            if (bytes == null) return null;
            return XmlParser.UTF8.GetString(bytes);
        }
        /// <summary>pa
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadFileFromPersistentPath(string path)
        {
            path = Path.Combine(Application.persistentDataPath, path);
            if (!File.Exists(path)) return null;

            byte[] data = null;
            using (var file = System.IO.File.OpenRead(path))
            {
                using (var br = new System.IO.BinaryReader(file))
                {
                    data = br.ReadBytes((int)file.Length);
                }
            }
            Debug.Log(string.Format("ReadFrom:{0} Data:{1}", path, data.Length));
            return data;
        }
        public static string GetPersistentPath(string path)
        {

            return Path.Combine(Application.persistentDataPath, path);
        }
    }

    public class BinaryFile
    {
        public BinaryFile(string filename)
        {
            this.FileName = filename;
        }
        public string FileName { set; get; }
        public byte[] Content { set; get; }
    }
    public class TextFile
    {
        public TextFile(string filename)
        {
            this.FileName = filename;
        }
        public string FileName { set; get; }
        public string Content { set; get; }
    }

}
