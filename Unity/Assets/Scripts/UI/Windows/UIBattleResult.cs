using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Proto;

namespace Assets.Scripts.UI.Windows
{
    partial class UIBattleResult
    {
        public class PackageGridTableModel : TableItemModel<PackageGridTableTemplate>
        {
            public PackageGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }
        public class DropGridTableModel : TableItemModel<DropGridTableTemplate>
        {
            public DropGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }

        public override void InitModel()
        {
            base.InitModel();
			bt_close.OnMouseClick ((s, e) => {
				this.HideWindow();
			});
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
        }

		public override void OnUpdateUIData ()
		{
			base.OnUpdateUIData ();
		}
        public override void OnHide()
        {
            base.OnHide();
        }

		public void ShowResult(int mapID, List<Item> item)
		{
			//show
		}
    }
}