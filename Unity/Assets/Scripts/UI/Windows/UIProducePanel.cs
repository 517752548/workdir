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
            public RewardItemGridTableModel(){}
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
            public ExcelConfig.ResourcesProduceConfig Config { get; set; }
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
            RewardItemGridTableManager.Count = havePeopleProduces.Count;
            index = 0;
            foreach(var i in RewardItemGridTableManager)
            {
                i.Model.Config = havePeopleProduces[index];
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
}