using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UILogin")]
    partial class UILogin : UIAutoGenWindow
    {
        public class GridTableTemplate : TableItemTemplate
        {
            public GridTableTemplate(){}
            public UILabel Label;

            public override void InitTemplate()
            {
                Label = FindChild<UILabel>("Label");

            }
        }


        public UIGrid Grid;


        public UITableManager<AutoGenTableItem<GridTableTemplate, GridTableModel>> GridTableManager = new UITableManager<AutoGenTableItem<GridTableTemplate, GridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            Grid = FindChild<UIGrid>("Grid");

            GridTableManager.InitFromGrid(Grid);

        }       
    }
}