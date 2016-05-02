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

            
            var list = new List<int>();
            if (string.IsNullOrEmpty(inputStr)) return list;
            var strs = inputStr.Split('|');
            foreach (var i in strs)
            {
                int id = 0;
                if (int.TryParse(i, out id))
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

        /// <summary>
        ///  1：2|1：2 to keyvalue [1,2]  [1,2]
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static List<SplitKeyValue> SplitKeyValues(string inputs)
        {
            var list = new List<SplitKeyValue>();
            if (string.IsNullOrEmpty(inputs)) return list;

            var strs = inputs.Split('|');
            foreach (var i in strs)
            {
                var t = i.Replace("：", ":");
                if (t.IndexOf(':') == -1)
                {
                    GameDebug.LogError("Not found Key of ':' in " + t);
                    continue;
                }
                var keyValue = t.Split(':');
                if (keyValue.Length == 2)
                {
                    int key = 0;
                    int value = 0;
                    SplitKeyValue kv = new SplitKeyValue();
                    if (!int.TryParse(keyValue[0], out key))
                    {
                        GameDebug.LogError("Can't ParseToInt:" + keyValue[0]);
                        continue;
                    }
                    if (!int.TryParse(keyValue[1], out value))
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
                    GameDebug.LogError("Can't ParseToInt:" + t);
                }
            }
            return list;
        }

        public static int ConvertToInt(string id)
        {
			int r = 0;
			if (int.TryParse (id, out r))
				return r;
			else {
				if (string.IsNullOrEmpty (id))
					return 0;
				GameDebug.LogError(id+" can't convert to int!");
			}
			return r;
        }

		public static float ConvertToFloat(string id)
		{
			float r = 0;
			if (float.TryParse (id, out r))
				return r;
			else {
				if (string.IsNullOrEmpty (id))
					return 0;
				GameDebug.LogError(id+" can't convert to int!");
			}
			return r;	
		}

        public static List<SplitKeyValue> SplitKeyValues(string keys, string values)
        {
            var keyValue = SplitIDS(keys);
            var vValue = SplitIDS(values);
            var list = new List<SplitKeyValue>();
            if (keyValue.Count != vValue.Count) return list;
            for (var i = 0; i < keyValue.Count; i++)
            {
                var v = new SplitKeyValue { Key = keyValue[i], Value = vValue[i] };
                list.Add(v);
            }
            return list;
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
