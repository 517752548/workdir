using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIArmyHouseSelect
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
                this.Item.Root.OnMouseClick((s, e) => {
                    UIArmyLevelUp.Show();
                });
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            ItemGridTableManager.Count = 5;
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}