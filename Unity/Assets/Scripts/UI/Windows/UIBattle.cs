using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Assets.Scripts.Combat.Battle.States;
using ExcelConfig;
using Assets.Scripts.Combat.Battle.Elements;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{

    partial class UIBattle : IBattleRender
    {
        public class StarGridTableModel : TableItemModel<StarGridTableTemplate>
        {
            public StarGridTableModel() { }
            public override void InitModel()
            {
                //todo
            }
        }
        public class SkillGridTableModel : TableItemModel<SkillGridTableTemplate>
        {
            public SkillGridTableModel() { }
            public override void InitModel()
            {
                //todo

                Template.Bt_skill.OnMouseClick((s, e) =>
                {
                    if (OnClick == null) return;
                    OnClick(this);
                });
            }

            public Action<SkillGridTableModel> OnClick;

            public int Index { get; set; }


            private Combat.Battle.Elements.BattleSoldier _Soldier { get; set; }
            public Combat.Battle.Elements.BattleSoldier Soldier
            {
                get { return _Soldier; }
                set
                {
                    _Soldier = value;
                    Template.Bt_skill.Text(value.SkillConfig.Name);
                    DataManagers.PlayerArmyManager.Singleton.SetJob(Template.job, value.Config);
                }
            }

            internal void Update()
            {
                Template.Bt_skill.Disable(_Soldier.LeftTime > 0);
            }
        }



        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_battleMode.OnMouseClick((s, e) =>
            {
                var mode = DataManagers.GamePlayerManager.Singleton.ControlMode;
                var setmode = (mode == DataManagers.BattleControlMode.AUTO) ?
                    DataManagers.BattleControlMode.PLAYER :
                    DataManagers.BattleControlMode.AUTO;

                DataManagers.GamePlayerManager.Singleton.SetControlMode(setmode);

                //bt_battleMode.Text(LanguageManager.Singleton[setmode == DataManagers.BattleControlMode.AUTO ? "BATTLE_AUTO" : "BATTLE_PLAYER"]);
                if (this.Per == null) return;
                this.Per.ChangePlayerControllor(setmode == DataManagers.BattleControlMode.AUTO);
                //bt_battleMode.Text()
            });

            bt_close.OnMouseClick((s, e) => {

                this.Per.State.Enable = false;
                UIMessageBox.ShowMessage(LanguageManager.Singleton["CANCEL_TITLE"], LanguageManager.Singleton["CANCEL_MESSAGE"],
                    () => { _cancel = true; this.Per.State.Enable = true; }, () => { this.Per.State.Enable = true; });
               
            });

        }
        public override void OnShow()
        {
            base.OnShow();
            _cancel = false;
            SkillBar.value = 0;
        }
        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            ShowData();
        }

        private void ShowData()
        {
            if (Monster != null)
            {
                HpBar.value = (float)Monster.HP / (float)Monster.MaxHP;
                SkillBar.value = (Monster.Soldiers[0].CdTimeToFloat() - Monster.Soldiers[0].LeftTime) / Monster.Soldiers[0].CdTimeToFloat();
            }
            if (Player != null)
            {
                PlayerHpBar.value = (float)Player.HP / (float)Player.MaxHP;
                foreach (var i in SkillGridTableManager)
                {
                    i.Model.Update();
                }
            }
        }


        public void ShowPlayer(Combat.Battle.Elements.BattleArmy player)
        {
            Player = player;
            var index = 0;
            SkillGridTableManager.Count = player.Soldiers.Count;
            foreach (var i in SkillGridTableManager)
            {
                i.Model.Index = index;
                i.Model.Soldier = player.Soldiers[index];
                i.Model.OnClick = OnClickItem;
                index++;
            }
            //throw new NotImplementedException();
        }

        private void OnClickItem(SkillGridTableModel obj)
        {
            tapIndex = obj.Index;
        }

        public void ShowMonster(Combat.Battle.Elements.BattleArmy monster)
        {
            Monster = monster;
            lb_monsterName.text = Monster.Soldiers[0].Config.Name;
            var config = monster.Soldiers[0].Config;
            DataManagers.PlayerArmyManager.Singleton.SetJob(jobicon, config);
            //throw new NotImplementedException();
        }

        private Assets.Scripts.Combat.Battle.Elements.BattleArmy Monster;
        private Assets.Scripts.Combat.Battle.Elements.BattleArmy Player;

        private int tapIndex = -1;

        public int GetTapIndex()
        {
            return tapIndex;
        }

        public int ReleaseTapIndex()
        {
            return tapIndex = -1;
        }

        private BattlePerception Per;
        public void SetPerception(BattlePerception per)
        {
            Per = per;
        }


        public void ShowBattleName(BattleConfig config)
        {
            lb_title.text = config.Name;
        }
        public void ShowDialog(ExcelConfig.BattleConfig battleConfig)
        {
            Per.State.Enable = false;
            UI.Windows.UIMessageBox.ShowMessage(battleConfig.Name, battleConfig.Dialog,
                () => { Per.State.Enable = true; },
                () => { Per.State.Enable = true; });
            //UITipDrawer.Singleton.DrawNotify(battleConfig.Dialog);
        }




        public void OnAttack(Combat.Battle.Elements.DamageResult result,BattleArmy cur)
        {
            if (cur.Camp == Proto.ArmyCamp.Player)
            {
                if (MonsterRoot.GetComponent<iTween>() == null)
                    iTween.shake(MonsterRoot.gameObject, 0.3f, 0.2f, new UnityEngine.Vector3(30, 20, 0));

                var an = daoguangFX.GetComponent<Animator>();
                an.SetTrigger("Start");
                UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["ATTACK_MONSTER"], result.Damage));
            }
            else
            {
                var an = this.zhuahenFx.GetComponent<Animator>();
                an.SetTrigger("Start");
                UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["LOST_HP"], result.Damage));
            }
        }

        private bool _cancel = false;

        public bool Cancel
        {
            get { return _cancel; }
        }
    }
}