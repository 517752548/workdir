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

                    if (Config == null) return;
                    UIControllor.Singleton.ShowMessage(LanguageManager.ReplaceEc(Config.Description));
                });
            }

            public Action<ItemGridTableModel> OnClickAdd;
            public Action<ItemGridTableModel> OnClickSub;

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

        private void ShowState()
        {
            lb_worker.text = string.Format(LanguageManager.Singleton["UIProduce_worker"],
               DataManagers.GamePlayerManager.Singleton.People - DataManagers.GamePlayerManager.Singleton.BusyPeople,
                DataManagers.GamePlayerManager.Singleton.People);

            var allOpenProduce = DataManagers.GamePlayerManager.Singleton.OpenProduceConfigs();
            var sb = new StringBuilder();
            var items = new Dictionary<int, int>();
            var list = lb_reward_list.GetComponent<UITextList>();
            list.Clear();
            foreach (var i in allOpenProduce)
            {
                var cost = Tools.UtilityTool.SplitKeyValues(i.CostItems);
                var reward = Tools.UtilityTool.SplitKeyValues(i.RewardItems);
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
            var listID = items.Keys.ToList();
            
            listID.Sort((l, r) => {
                if (l > r) return 1;
                if (l == r) return 0; return -1;
            });

            
            foreach (var i in listID)
            { 
                var name = string.Empty;
                var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>(i);
                if (config != null)
                    name = config.Name;
                list.Add(string.Format(LanguageManager.Singleton["PRODUCE_ITEM"], name, items[i]));
            }

        }
    }
}