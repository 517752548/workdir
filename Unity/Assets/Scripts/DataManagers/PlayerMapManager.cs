using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataManagers
{
    public class PlayerMapManager:Tools.XSingleton<PlayerMapManager>, IPresist
    {
        public void Presist()
        {
            Tools.PresistTool.SaveJson(MapIDS.ToList(),MAP_LIST_FILE);
            foreach(var i in _maps)
            {
                if (!i.Value.IsChanged) continue;
                Tools.PresistTool.SaveJson(i.Value, GetMapFileName(i.Value.MapID));
            }
        }

        public void Load()
        {
            //throw new NotImplementedException();
            var ids = Tools.PresistTool.LoadJson<List<int>>(MAP_LIST_FILE);
            MapIDS = new HashSet<int>();
            if(ids!=null)
            {
                foreach(var i in ids)
                {
                    if (MapIDS.Contains(i)) continue;
                    MapIDS.Add(i);
                }
            }

            _maps = new Dictionary<int, MapPresistData>();
            foreach(var i in MapIDS)
            {
                var data = Tools.PresistTool.LoadJson<MapPresistData>(GetMapFileName(i));
                _maps.Add(data.MapID, data);
            }
        }

        public void Reset()
        {
            MapIDS = new HashSet<int>();
            foreach(var i in _maps)
            {
                i.Value.IsChanged = true;
                i.Value.Posistions.Clear();
            }
            Presist();
        }

        public const string MAP_FORMAT_STR = "__MAP__{0:0000}.json";
        public const string MAP_LIST_FILE = "__MAP_LIST.json";

		public void RecordMap(int mapID, int index, bool isOpen, string json)
		{
			MapPresistData map;
			if (_maps.TryGetValue (mapID, out map)) {
				if (map [index] == null) {
					map [index] = new MapPosData {  
						Index = index, Json = json, IsOpened = isOpen
					};
				} else {
					map [index].Json = json;

				}
				map.IsChanged = true;
			} else {
				var data = CreateMapPresistData (mapID);
				data [index] = new MapPosData { Index = index, Json = json, IsOpened = isOpen };
			}


		}

        private MapPresistData  CreateMapPresistData(int mapID)
        {
			var map = new MapPresistData () { MapID = mapID, 
				Posistions = new Dictionary<int, MapPosData> (),
				IsChanged = true
			};
            _maps.Add(mapID, map);
            return map;
        }
		public bool ReadMap(int mapID, int index, out MapPosData resdata)
        {
			resdata = null;
            MapPresistData map;
            if(_maps.TryGetValue(mapID, out map))
            {
                var data = map[index];
                if (data != null) { 
					resdata = data ; 
					return true;
				}
            }
            return false;
        }

        private Dictionary<int, MapPresistData> _maps = new Dictionary<int, MapPresistData>();
        
        public static string GetMapFileName(int map)
        {
            return string.Format(MAP_FORMAT_STR, map);
        }

        private HashSet<int> MapIDS { set; get; }


		public void OpenClosedIndex(int mapID, int index, GameMap map)
		{
			//size ;
			var size = 1;
			var current = GamePlayerManager.IndexToPos (index);

			for (int x = -size; x <= size; x++) {
				for (int y = -size; y <= size; y++) {
					var pos = new UnityEngine.Vector2 (x+ current.x, y+current.y);
					if (map.HaveIndex(pos)) {
						//var recordValue = new MapPosData
						RecordMap (mapID, 
							GamePlayerManager.PosXYToIndex ((int)pos.x, (int)pos.y), 
							true,"");

					}
				}
			}
		}


		public bool IsOpen(int mapID,int index)
		{
			MapPosData data;
			if (ReadMap (mapID, index, out data)) {
			
				return data.IsOpened;
			}
			return false;
		}

     
    }


    public class MapPresistData
    {
        [JsonName("M")]
        public int MapID { set; get; }

        [JsonName("PS")]
        public List<MapPosData> SavePosistions
        {
            set
            {
                Posistions = new Dictionary<int, MapPosData>();
                foreach (var i in value)
                {
                    Posistions.Add(i.Index, i);
                }
            }
            get
            {
                return Posistions.Values.ToList();
            }
        }

        [JsonIgnore]
        public Dictionary<int, MapPosData> Posistions { set; get; }

        [JsonIgnore]
        public bool IsChanged { set; get; }

		[JsonIgnore]
        public MapPosData this[int index]
        {
            set
            {
                if (Posistions.ContainsKey(index))
                    Posistions[index] = value;
                else
                    Posistions.Add(index, value);
            }
            get
            {
                if (Posistions.ContainsKey(index))
                    return Posistions[index];
                else
                    return null;
            }
        }

        public MapPresistData()
        {
            IsChanged = false;
            Posistions = new Dictionary<int, MapPosData>();
        }
    }

    public class MapPosData{
        [JsonName("I")]
        public int Index { set; get; }
		[JsonName("O")]
		public bool IsOpened{ set; get; }
        [JsonName("V")]
        public string Json { set; get; }
    }
}
