using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    partial class UIProducePanel
    {
		public class ProduceListGridTableModel:TableItemModel<ProduceListGridTableTemplate>
		{

			public ProduceListGridTableModel()
			{}
			public override void InitModel ()
			{
				
			}

			private string _text;
			public string Text
			{ 
				set
				{
					_text = value;
					Template.lb_reward_list.text = _text;
				} 
				get{ return _text;} 
			}

			public  void SetDrag(bool can)
			{
				var d = this.Template.lb_reward_list.GetComponent<UIDragScrollView>();
				d.enabled = can;
			}
		}

        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
                Template.bt_add.OnMouseClick((s, e) =>
                {
                    if (OnClickAdd == null) return;
                    OnClickAdd(this);
                });
                //todo
                Template.bt_cal.OnMouseClick((s, e) =>
                {
                    if (OnClickSub == null) return;
                    OnClickSub(this);
                });

                Template.bt_item.OnMouseClick((s, e) => {
                    if (OnClickItem == null) return;

                    OnClickItem(this);
                });

                
            }

            public Action<ItemGridTableModel> OnClickItem;
            public Action<ItemGridTableModel> OnClickAdd;
            public Action<ItemGridTableModel> OnClickSub;

            public void SetDrag(bool canDrag)
            {
                var d = this.Item.Root.GetComponent<UIDragScrollView>();
                d.enabled = canDrag;
            }
            private ExcelConfig.ResourcesProduceConfig _Config { get; set; }

            public ExcelConfig.ResourcesProduceConfig Config
            {
                get { return _Config; }
                set
                {
                    _Config = value;
                    var prp = DataManagers.GamePlayerManager.Singleton.GetProducePeople(_Config.ID);
                    Template.lb_name.text = Config.Name;
                    Template.Label.text = string.Format("{0:N0}", prp);
                }
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_close.OnMouseClick((s, e) =>
            {
                HideWindow();
            });
        }
        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();

            lb_reward_time.text = "";
            var allOpenProduce = DataManagers.GamePlayerManager.Singleton.OpenProduceConfigs();
            ItemGridTableManager.Count = allOpenProduce.Count;
            int index = 0;
            foreach (var i in ItemGridTableManager)
            {
                i.Model.Config = allOpenProduce[index];
                i.Model.OnClickAdd = ClickAdd;
                i.Model.OnClickSub = ClickSub;
                i.Model.OnClickItem = ClickItem;
                i.Model.SetDrag(allOpenProduce.Count >= 7);
                index++;
            }
            
            ShowState();
        }

        private void ClickAdd(ItemGridTableModel obj)
        {
            DataManagers.GamePlayerManager.Singleton.AddPeopleOnProduce(obj.Config.ID, 1);
            obj.Config = obj.Config;
            ShowState();
        }

        private void ClickSub(ItemGridTableModel obj)
        {
            DataManagers.GamePlayerManager.Singleton.CalPeopleOnProduce(obj.Config.ID, 1);
            obj.Config = obj.Config; ShowState();
            ShowState();
        }

        //Ã¿Ãë¸üÐÂ
        public override void OnPreScecondUpdate()
        {
            base.OnPreScecondUpdate();
            var time = DataManagers.GamePlayerManager.Singleton.TimeToProduce;

            lb_reward_time.text = string.Format(LanguageManager.Singleton["PRODUCE_TIME"], Mathf.FloorToInt((float)time.TotalMinutes), time.Seconds);
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        private float hideTime = 0;
        public void ClickItem(ItemGridTableModel item)
        {

            var Config = item.Config;
            if (Config == null) return;
            UIControllor.Singleton.ShowMessage( LanguageManager.ReplaceEc(Config.Description),4);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        private void ShowState()
        {
            lb_worker.text = string.Format(LanguageManager.Singleton["UIProduce_worker"],
               DataManagers.GamePlayerManager.Singleton.People - DataManagers.GamePlayerManager.Singleton.BusyPeople,
                DataManagers.GamePlayerManager.Singleton.People);

            if (DataManagers.GamePlayerManager.Singleton.People - DataManagers.GamePlayerManager.Singleton.BusyPeople == 0)
            {
                UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["BUILD_FRO_MORE_PEOPLE"], -1);
            }
            var allOpenProduce = DataManagers.GamePlayerManager.Singleton.OpenProduceConfigs();
            var sb = new StringBuilder();
            var items = new Dictionary<int, int>();
            //var list = lb_reward_list.GetComponent<UITextList>();
            //list.Clear();
            foreach (var i in allOpenProduce)
            {
                var cost = Tools.UtilityTool.SplitKeyValues(i.CostItems,i.CostItemsNumber);
                var reward = Tools.UtilityTool.SplitKeyValues(i.RewardItems,i.RewardItemsNumber);
                foreach (var c in cost)
                {
                    var p = DataManagers.GamePlayerManager.Singleton.GetProducePeople(i.ID) * c.Value;
                    if (p == 0) continue;
                    if(items.ContainsKey(c.Key))
                    {
                        items[c.Key] -= p;
                    }
                    else
                    {
                        items.Add(c.Key, -p);
                    }
                }
                foreach (var c in reward)
                {
                    var p = DataManagers.GamePlayerManager.Singleton.GetProducePeople(i.ID) * c.Value;
                    if (p == 0) continue;
                    if (items.ContainsKey(c.Key))
                    {
                        items[c.Key] += p;
                    }
                    else
                    {
                        items.Add(c.Key, p);
                    }
                }
            }
            var listID = items.Where(t=>t.Value !=0).Select(t=>t.Key).ToList();
            
            listID.Sort((l, r) => {
                if (l > r) return 1;
                if (l == r) return 0; return -1;
            });

			int lastCout = ProduceListGridTableManager.Count;
			ProduceListGridTableManager.Count = listID.Count;
			int index = 0;
            foreach (var i in listID)
            { 
				
                var name = string.Empty;
                var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>(i);
                if (config != null)
                    name = config.Name;
				ProduceListGridTableManager[index].Model.Text=
					(string.Format(LanguageManager.Singleton["PRODUCE_ITEM"], name, items[i]));
				ProduceListGridTableManager [index].Model.SetDrag (listID.Count >= 12);

				index++;
            }

			if (lastCout != ProduceListGridTableManager.Count)
				this.ListViewS.ResetClip ();

			OnPreScecondUpdate ();

        }
    }
}