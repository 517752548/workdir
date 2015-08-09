using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIExploreResources
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            bt_close.OnMouseClick((s, e) => {

                this.HideWindow();
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            this.ItemGridTableManager.Count = 20;
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}