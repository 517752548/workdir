using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;

namespace Assets.Scripts.UI.Windows
{
    partial class UIArmyTrain
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }

            public Action<ItemGridTableModel> OnClick;
            private ArmyData _data;

            public ArmyData Data
            {
                set
                {
                    _data = value;
                    switch (Data.Type)
                    {
                        case ArmyData.OpType.LevelUP:
                            //Template.lb_skill.text = string.Format(LanguageManager.Singleton["SOLDIER_SKILL"], Data.Soldier.Config.SkillName);
                            //Template.lb_hp.text = string.Format(LanguageManager.Singleton["SOLDIER_HP"], Data.Soldier.Config.HPMax);
                            //Template.lb_damage.text = string.Format(LanguageManager.Singleton["SOLDIER_DAMAGE"], Data.Soldier.Config.Damage);
                            //Template.lb_speed.text = string.Format(LanguageManager.Singleton["SOLDIER_SPEED"], Data.Soldier.Config.AttackSpeed);
                            Template.lb_armyName.text = string.Format(LanguageManager.Singleton["SOLDIER_NAME"],
                                                    Data.Soldier.Config.Name, Data.Soldier.Number);
                            break;
                        case ArmyData.OpType.Train:
                            Template.lb_damage.text = Template.lb_hp.text = Template.lb_skill.text = Template.lb_speed.text = string.Empty;
                            Template.lb_armyName.text = LanguageManager.Singleton["TRAIN_BT_NAME"];
                            break;

                    }

                }
                get
                {
                    return _data;
                }
            }
            public override void InitModel()
            {
                //todo
                Template.bt_item.OnMouseClick((s,e)=>{
                    if(OnClick==null) return;
                    OnClick(this);
                });
            }

            internal void SetDrag(bool p)
            {
                var drag = this.Item.Root.GetComponent<UIDragScrollView>();
                if (drag != null)
                    drag.enabled = p;
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            this.bt_left.OnMouseClick((s, e) =>
            {
                this.HideWindow();
                var ui = UIManager.Singleton.CreateOrGetWindow<UICastlePanel>();
                ui.ShowWindow();
            });
            this.bt_right.ActiveSelfObject(false);
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

        //update ui data
        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            //TRAIN_BT_NAME
            var list = new List<ArmyData>() { new ArmyData { Soldier = null, Type = ArmyData.OpType.Train } };
            var soldiers = DataManagers.PlayerSoldierManager.Singleton.Soldiers;
            foreach (var i in soldiers)
            {
                list.Add(new ArmyData
                {
                    Type = ArmyData.OpType.LevelUP,
                    Soldier = i
                });
            }
            ItemGridTableManager.Count = list.Count;
            int index = 0;
            foreach (var i in ItemGridTableManager)
            {
                i.Model.Data = list[index];
                i.Model.SetDrag(list.Count >= 10);
                i.Model.OnClick = OnItemClick;
                index++;
            }


        }

        private void OnItemClick(ItemGridTableModel model) 
        {
           //Show
            int id = model.Data.Type == ArmyData.OpType.Train? -1: model.Data.Soldier.SoldierID;
            //¥¶¿Ìƒ¨»œ -1
            var levelUpSoldiers = ExcelToJSONConfigManager.Current
                .GetConfigs<MonsterLvlUpConfig>((t) => {
                    if (t.OldMonster == id) return true;
                    return false;
                });
            
            //frist 

            if (levelUpSoldiers ==null || levelUpSoldiers.Length == 0)
            {
                UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["MAX_LEVEL_SOLDIER"]);
                return;
            }
            //test
            if (DataManagers.PlayerSoldierManager.Singleton.TrainSoldier(Tools.GRandomer.RandomArray(levelUpSoldiers)))
            {
                UI.UIManager.Singleton.OnUpdateUIData();
            }
        }

        public class ArmyData
        {
            public enum OpType
            {
                LevelUP =1,
                Train =2
            }
            public OpType Type { set; get; }
            public DataManagers.PlayerSoldierData Soldier { set; get; } 
        }

    }
}