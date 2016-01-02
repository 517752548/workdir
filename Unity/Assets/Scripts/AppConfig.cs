using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Assets.Scripts
{
	public class AppConfig : Tools.XSingleton<AppConfig>
	{
		private Dictionary<string, string> keys = new Dictionary<string, string> ();

		public const string APP_CONFIG_PATH="/configs.xml";


		public bool GetKeyBoolValue(string key)
		{
			var v = this [key];
			if (!string.IsNullOrEmpty (v)) 
			{
				return "true".Equals( v, System.StringComparison.CurrentCultureIgnoreCase);
			}
			return false;
		}

		public int GetKeyIntValue(string key)
		{
			var v = this [key];
			if (!string.IsNullOrEmpty (v)) {
				return System.Convert.ToInt32 (v);
			}
			return -1;
		}

		public string this[string key]
		{
			get{ 
				if (keys.Count == 0) {
					var str = App.GameAppliaction.Singleton.ReadStreamingFile (APP_CONFIG_PATH);
					if (string.IsNullOrEmpty (str))
						return null;
					var data = Tools.XmlParser.DeSerialize<List<KeyValue>> (str);
					foreach (var i in data) {
						keys.Add (i.Key, i.Value);
					}
				}
				string value;
				if (keys.TryGetValue (key, out value))
					return value;
				return null;
			}
		}
	}

	public class KeyValue{
		[XmlAttributeAttribute("K")]
		public string Key{ set; get; }
		[XmlText]
		public string Value{ set; get; }
	}
}