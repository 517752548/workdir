using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    partial class UICastlePanel
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
            }

            public DataManagers.PlayerGameItem _GameItem { get; set; }

            public DataManagers.PlayerGameItem GameItem
            {
                get { return _GameItem; }
                set
                {
                    _GameItem = value;
                    Template.lb_count.text = string.Format("{0:N0}", _GameItem.Num);
                    Template.lb_name.text = _GameItem.Config.Name;
                }
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            bt_produce.OnMouseClick((s, e) => {
                var cd = DataManagers.GamePlayerManager.Singleton.CallProduceGold();
                cdTime = cd + Time.time;
                bt_produce.Disable(true);
            });
            //Write Code here
        }

        private float cdTime = -1f;
        public override void OnShow()
        {
            base.OnShow();
            lb_time.text = string.Empty;
            OnUpdateUIData();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            var allItem = DataManagers.PlayerItemManager.Singleton.GetAllItems();
            var index = 0;
            ItemGridTableManager.Count = allItem.Count;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.GameItem = allItem[index];
                index++;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(cdTime>0)
            {
                if(cdTime <Time.time)
                {
                    cdTime = -1f;
                    bt_produce.Disable(false);
                    lb_time.text = string.Empty;
                }
                else
                {
                    lb_time.text = string.Format("{0:0.00}", cdTime - Time.time);
                }
            }
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}