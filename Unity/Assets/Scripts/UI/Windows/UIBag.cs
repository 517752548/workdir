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
            public GridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });
        }
        public override void OnShow()
        {
            base.OnShow();
            this.GridTableManager.Count = 5;
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}