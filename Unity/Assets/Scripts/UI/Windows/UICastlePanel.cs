using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Assets.Scripts.Appliaction;

namespace Assets.Scripts.UI.Windows
{
    partial class UICastlePanel
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

            bt_produce.OnMouseClick((s, e) =>
            {
                //onclick 
                var produceID = GameAppliaction.Singleton.ConstValues.PRODUCE_ITEM_ID;
                
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            ItemGridTableManager.Count = 10;
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}