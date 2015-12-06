using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIAchievement
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
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
            bt_return.OnMouseClick((s, e) => { HideWindow(); });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();

            
            ItemGridTableManager.Count = 10;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.SetDrag(ItemGridTableManager.Count >= 15);
            }
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}