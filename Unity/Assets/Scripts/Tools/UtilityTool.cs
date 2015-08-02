using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Tools
{
    public class UtilityTool
    {
        public static List<int> SplitIDS(string inputStr)
        {
            var strs = inputStr.Split('|');
            var list = new List<int>();
            foreach(var i in strs)
            {
                int id = 0;
                if(int.TryParse(i,out id))
                {
                    list.Add(id);
                }
                else
                {
                    GameDebug.LogError("TRY To Parse Int:" + i);
                }
            }
            return list;
        }

        public static List<SplitKeyValue> SplitKeyValues(string inputs)
        {
            var strs = inputs.Split('|');
            var list = new List<SplitKeyValue>();
            foreach(var i in  strs)
            {
                if(i.IndexOf(':')==-1)
                {
                    GameDebug.LogError("Not found Key of ':' in " + i);
                    continue;
                }
                var keyValue = i.Split('|');
                if(keyValue.Length ==2)
                {
                    int key = 0;
                    int value = 0;
                    SplitKeyValue kv = new SplitKeyValue();
                    if(!int.TryParse(keyValue[0],out key))
                    {
                        GameDebug.LogError("Can't ParseToInt:" + keyValue[0]);
                        continue;
                    }
                    if(!int.TryParse(keyValue[1],out value))
                    {
                        GameDebug.LogError("Can't ParseToInt:" + keyValue[1]);
                        continue;
                    }

                    kv.Key = key;
                    kv.Value = value;
                    list.Add(kv);
                }
                else
                {
                    GameDebug.LogError("Can't ParseToInt:" + i);
                }
            }
            return list;
        }

        public static int ConvertToInt(string id)
        {
            return Convert.ToInt32(id);
        }
    }

    /// <summary>
    /// 9:2|2:2
    /// </summary>
    public class SplitKeyValue
    {
        public int Key { set; get; }
        public int Value { set; get; }
    }
}
