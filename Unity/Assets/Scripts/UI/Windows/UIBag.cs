using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIBag
    {
        public class GridTableModel : TableItemModel<GridTableTemplate>
        {
            public GridTableModel() { }
            public override void InitModel()
            {
                //todo
            }

            private DataManagers.PlayerGameItem _GameItem;
            public DataManagers.PlayerGameItem GameItem
            {
                get
                {
                    return _GameItem;
                }
                set
                {
                    _GameItem = value;
                    Template.lb_name.text = value.Config.Name;
                    Template.lv_num.text = string.Format(string.Format(LanguageManager.Singleton["APP_NUM_FORMAT"], value.Num));
                }
            }

            public void SetDrag(bool canDrag)
            {
               var d = this.Item.Root.GetComponent<UIDragScrollView>();
               d.enabled = canDrag;
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
            var allItem = DataManagers.PlayerItemManager.Singleton.GetAllItems();
            this.GridTableManager.Count = allItem.Count;
            int index = 0;
            foreach (var i in GridTableManager)
            {
                i.Model.GameItem = allItem[index];
                i.Model.SetDrag(allItem.Count >= 12);
                index++;
            }
        }

        public override void OnHide()
        {
            base.OnHide();
        }
    }
}