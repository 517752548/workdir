using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using UnityEngine;
using ExcelConfig;

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
                Template.lb_add.OnMouseClick((s, e) => {
                    if (OnAdd == null) return;
                    OnAdd(this);
                });
                Template.lb_cal.OnMouseClick((s, e) => {
                    if (OnCal == null) return;
                    OnCal(this);
                });
            }
            private ExcelConfig.ResourcesProduceConfig _Config;
            private DataManagers.ProducePrisitData Produce;
            public ExcelConfig.ResourcesProduceConfig Config
            {
                get { return _Config; }
                set {
                    _Config = value;
                    Produce = DataManagers.GamePlayerManager.Singleton.GetProduceStateByID(value.ID);
                    Template.Label.text = _Config.Name;
                    Template.lb_num.text = string.Format("{0:0}", Produce.PeopleNum);
                }
            }

            public Action<ItemGridTableModel> OnAdd;
            public Action<ItemGridTableModel> OnCal;
            internal void SetDrag(bool p)
            {
                var drag = this.Item.Root.GetComponent<UIDragScrollView>();
                if (drag != null)
                    drag.enabled = p;
            }
        }
        public class RewardItemGridTableModel : TableItemModel<RewardItemGridTableTemplate>
        {
            public RewardItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
            }
            internal void SetDrag(bool p)
            {
                var drag = this.Item.Root.GetComponent<UIDragScrollView>();
                if (drag != null)
                    drag.enabled = p;
            }

            private DisplayItem _DItem;
            public DisplayItem DItem
            {
                get
                {
                    return _DItem;
                }
                set
                {
                    _DItem = value;
                    Template.lb_name.text = _DItem.Config != null ? _DItem.Config.Name : "";
                    Template.lb_count.text = string.Format("{0}", _DItem.Produce);
                }
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            this.bt_left.OnMouseClick((s, e) =>
            {
                this.HideWindow();
                var ui = UIManager.Singleton.CreateOrShowWindow<UICastlePanel>();
                ui.ShowWindow();
            });
            this.bt_right.ActiveSelfObject(false);
            ItemGridTableManager.AutoLayout = false;
        }
        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
            ItemGridTableManager.RepositionLayout();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnPreScecondUpdate()
        {
            base.OnPreScecondUpdate();
            var time = DataManagers.GamePlayerManager.Singleton.TimeToProduce;
            lb_timeLimit.text = string.Format(LanguageManager.Singleton["PRODUCE_TIME"], Mathf.Floor((float)time.TotalMinutes), time.Seconds);
        }
        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            lb_name_lb.text = LanguageManager.Singleton["PEOPLE_NAME"];
            var produces = DataManagers.GamePlayerManager.Singleton.OpenProduceConfigs();
            var people = DataManagers.GamePlayerManager.Singleton[DataManagers.PlayDataKeys.PEOPLE_COUNT];
            var inWorkPeople = DataManagers.GamePlayerManager.Singleton.CalInWorkPeople();
            lb_people.text = string.Format(LanguageManager.Singleton["PRODUCE_PEOPLE"], inWorkPeople, people);
            var time =DataManagers.GamePlayerManager.Singleton.TimeToProduce;
            lb_timeLimit.text = string.Format(LanguageManager.Singleton["PRODUCE_TIME"], 
                Mathf.Floor((float)time.TotalMinutes),time.Seconds );
            produces.Sort((r, l) => {
                if (r.ID > l.ID) return 1;
                if (r.ID == l.ID) return 0;
                return -1;
            });
            ItemGridTableManager.Count = produces.Count;
            int index = 0;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.Config = produces[index];
                i.Model.SetDrag(produces.Count >= 9);
                i.Model.OnAdd = OnAdd;
                i.Model.OnCal = OnCal;
                index++;
            }
            var havePeopleProduces = produces.Where(t => DataManagers.GamePlayerManager.Singleton.GetProducePeople(t.ID) > 0).ToList();
            var itemProduces = new Dictionary<int,DisplayItem>();
            foreach(var i in havePeopleProduces)
            {
                var rewards = DataManagers.PlayerItemManager.SplitFormatItemData(i.RewardItems);
                var requires = DataManagers.PlayerItemManager.SplitFormatItemData(i.CostItems);

                foreach(var r in rewards)
                {
                    var peopleNum = DataManagers.GamePlayerManager.Singleton.GetProducePeople(i.ID);
                    if(!itemProduces.ContainsKey(r[0]))
                    {
                        itemProduces.Add(r[0], new DisplayItem
                        {
                            Produce = r[1] * peopleNum,
                            Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(r[0])
                        });
                    }
                    else
                    {
                        itemProduces[r[0]].Produce += r[1] * peopleNum;
                    }
                }

                foreach(var r in requires)
                {
                    var peopleNum = DataManagers.GamePlayerManager.Singleton.GetProducePeople(i.ID);
                    if (!itemProduces.ContainsKey(r[0]))
                    {
                        itemProduces.Add(r[0], new DisplayItem
                        {
                            Produce = -r[1] * peopleNum,
                            Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(r[0])
                        });
                    }
                    else
                    {
                        itemProduces[r[0]].Produce -= r[1] * peopleNum;
                    }
                }

            }

            var displayItems = itemProduces.Values.ToList();
            RewardItemGridTableManager.Count = displayItems.Count;
            index = 0;
            foreach(var i in RewardItemGridTableManager)
            {
                i.Model.DItem = displayItems[index];
                i.Model.SetDrag(displayItems.Count >= 6);
                index++;
            }
        }

        private void OnAdd(ItemGridTableModel item) 
        {
            DataManagers.GamePlayerManager.Singleton.AddPeopleOnProduce(item.Config.ID, 1);
            OnUpdateUIData();
        }

        private void OnCal(ItemGridTableModel item) 
        {
            DataManagers.GamePlayerManager.Singleton.CalPeopleOnProduce(item.Config.ID, 1);
            OnUpdateUIData();
        }
    }

    public class DisplayItem
    {
        public ItemConfig Config { set; get; }
        public int Produce { set; get; }
    }
}