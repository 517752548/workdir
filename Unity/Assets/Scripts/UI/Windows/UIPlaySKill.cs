using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIPlaySKill
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
				Template.Bt_itemName.OnMouseClick ((s, e) => {
					UIControllor.Singleton.ShowInfo(Config.Desctription);
				});
                //todo
            }

			private ExcelConfig.TalentConfig _Config; 
			public ExcelConfig.TalentConfig Config{
				set{ 
					_Config = value;
					Template.Bt_itemName.Text (_Config.Name);
					Template.s_completed.ActiveSelfObject (false);
				}
				get{return _Config;} 
			}
		}

        public override void InitModel()
        {
            base.InitModel();
			bt_return.OnMouseClick((s,e)=>{
				this.HideWindow();
			});
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
			OnUpdateUIData ();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

		public override void OnUpdateUIData ()
		{
			base.OnUpdateUIData ();
			var configs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.TalentConfig> ();
			ItemGridTableManager.Count = configs.Length;
			int index = 0;
			foreach (var i in ItemGridTableManager) {
				i.Model.Config = configs [index];
				index++;
			}
		}
    }
}