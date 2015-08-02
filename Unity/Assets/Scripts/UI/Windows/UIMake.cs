using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIMake
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }
        public class TypeItemGridTableModel : TableItemModel<TypeItemGridTableTemplate>
        {
            public TypeItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
                Template.Bt_itemName.OnMouseClick((s, e) => {
                    if (OnClick == null) return;
                    OnClick(this);
                });
            }

            public Action<TypeItemGridTableModel> OnClick;
        }

        public enum ShowTypeName
        {
            Types,
            Info
        }

        private ShowTypeName CurrentType = ShowTypeName.Types;
        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_close.OnMouseClick((s, e) => {
                 if(CurrentType == ShowTypeName.Info)
                 {
                     ShowTypes();
                 }
                 else { HideWindow(); }
            });
        }
        public override void OnShow()
        {
            base.OnShow();
            ShowTypes();
        }

        private void ShowTypes()
        {
            CurrentType = ShowTypeName.Types;
            PackageTypeView.ActiveSelfObject(true);
            PackageView.ActiveSelfObject(false);
            TypeItemGridTableManager.Count = 3;
            foreach (var i in TypeItemGridTableManager)
            {
                i.Model.OnClick = OnClick;
            }
        }
        private void OnClick(TypeItemGridTableModel obj)
        {
            ShowType(0);
        }
        public override void OnHide()
        {
            base.OnHide();
        }
        public void ShowType(int type)
        {
            CurrentType = ShowTypeName.Info;
            PackageTypeView.ActiveSelfObject(false);
            PackageView.ActiveSelfObject(true);
            ItemGridTableManager.Count = 5;
        }
    }
}