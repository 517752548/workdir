using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIArmyLevelUp")]
    partial class UIArmyLevelUp : UIAutoGenWindow
    {
        public class formStartGridTableTemplate : TableItemTemplate
        {
            public formStartGridTableTemplate(){}

            public override void InitTemplate()
            {

            }
        }
        public class toStartGridTableTemplate : TableItemTemplate
        {
            public toStartGridTableTemplate(){}

            public override void InitTemplate()
            {

            }
        }


        public UISprite character1;
        public UISprite character2;
        public UISprite character3;
        public UILabel lb_title;
        public UITexture formicon;
        public UIGrid formStartGrid;
        public UISprite formjob;
        public UILabel fromName;
        public UILabel from_des;
        public UILabel toName;
        public UITexture toicon;
        public UIGrid toStartGrid;
        public UISprite tojob;
        public UILabel to_des;


        public UITableManager<AutoGenTableItem<formStartGridTableTemplate, formStartGridTableModel>> formStartGridTableManager = new UITableManager<AutoGenTableItem<formStartGridTableTemplate, formStartGridTableModel>>();
        public UITableManager<AutoGenTableItem<toStartGridTableTemplate, toStartGridTableModel>> toStartGridTableManager = new UITableManager<AutoGenTableItem<toStartGridTableTemplate, toStartGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            character1 = FindChild<UISprite>("character1");
            character2 = FindChild<UISprite>("character2");
            character3 = FindChild<UISprite>("character3");
            lb_title = FindChild<UILabel>("lb_title");
            formicon = FindChild<UITexture>("formicon");
            formStartGrid = FindChild<UIGrid>("formStartGrid");
            formjob = FindChild<UISprite>("formjob");
            fromName = FindChild<UILabel>("fromName");
            from_des = FindChild<UILabel>("from_des");
            toName = FindChild<UILabel>("toName");
            toicon = FindChild<UITexture>("toicon");
            toStartGrid = FindChild<UIGrid>("toStartGrid");
            tojob = FindChild<UISprite>("tojob");
            to_des = FindChild<UILabel>("to_des");

            formStartGridTableManager.InitFromGrid(formStartGrid);
            toStartGridTableManager.InitFromGrid(toStartGrid);

        }
        public static UIArmyLevelUp Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIArmyLevelUp>();
            ui.ShowWindow();
            return ui;
        }
    }
}