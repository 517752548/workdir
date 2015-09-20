using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Tools
{
    public class PresistTool
    {
        public static void SaveJson<T>(T obj, string path)
        {
            var json = JsonTool.Serialize(obj);
            App.GameAppliaction.Singleton.SaveFile(path, json, false);
        }

        public static T LoadJson<T>(string path) where T:new()
        {
            var json = App.GameAppliaction.Singleton.ReadFile(path);
            if(!string.IsNullOrEmpty(json))
            {
                return JsonTool.Deserialize<T>(json);
            }
            return  new T();
        }
    }
}
