using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Assets.Scripts.Combat.Battle.States;
using ExcelConfig;
using Assets.Scripts.Combat.Battle.Elements;
using UnityEngine;
using System.Collections;

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
                    DataManagers.PlayerArmyManager.Singleton.SetIcon(Template.icon, value.Config, DataManagers.TypeOfIcon.BattleMin);
                }
            }

            internal void Update()
            {
                if (_Soldier == null) return;
				Template.s_mask.fillAmount = _Soldier.LeftTime / _Soldier.CdTimeToFloat ();

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
					//batte_player_controll_bt
				AutoSprite.spriteName =  
						setmode == Assets.Scripts.DataManagers.BattleControlMode.AUTO?
						"Battle_ui_bt_auto":"batte_player_controll_bt";
                //bt_battleMode.Text(LanguageManager.Singleton[setmode == DataManagers.BattleControlMode.AUTO ? "BATTLE_AUTO" : "BATTLE_PLAYER"]);
                if (this.Per == null) return;
                this.Per.ChangePlayerControllor(setmode == DataManagers.BattleControlMode.AUTO);
                //bt_battleMode.Text()
            });

            bt_close.OnMouseClick((s, e) => {

                this.Per.State.Enable = false;
                UIMessageBox.ShowMessage(LanguageManager.Singleton["CANCEL_TITLE"], 
					LanguageManager.Singleton["CANCEL_MESSAGE"],
                    () => { _cancel = true; this.Per.State.Enable = true; }, 
					() => { this.Per.State.Enable = true; });
               
            });

			bt_addHp.OnMouseClick ((s, e) => {
			  
				var foodEntry = App.GameAppliaction.Singleton.ConstValues.FoodItemID;
				var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(foodEntry);
				var hp = Tools.UtilityTool.ConvertToInt(config.Pars1);
				hp += DataManagers.GamePlayerManager.Singleton. FoodChargeAppend;
				bt_addHp.Disable(true);

				App.GameAppliaction.Singleton.DelayCall(()=>{bt_addHp.Disable(false);},1f);
				if(DataManagers.PlayerItemManager.Singleton.CalItemFromPack(foodEntry,1))
				{	
					Per.PlayerAddHp(hp);
					UIManager.Singleton.UpdateUIData();
				}
			});

        }
        public override void OnShow()
        {
            base.OnShow();
			MonsterRoot.ActiveSelfObject (false);
			_cancel = false;
			var mode = DataManagers.GamePlayerManager.Singleton.ControlMode;
			AutoSprite.spriteName =  
				mode == Assets.Scripts.DataManagers.BattleControlMode.AUTO?
				"Battle_ui_bt_auto":"batte_player_controll_bt";
			SkillBar.value = 0;
			OnUpdateUIData ();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();

			lb_foodNum.text = string.Format ("{0}", DataManagers.PlayerItemManager.Singleton.GetFoodNum ());
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
                targetMonsterHp = (float)Monster.HP / (float)Monster.MaxHP;
				SkillBar.value = Monster.IsDead?0:((Monster.Soldiers[0].CdTimeToFloat() - Monster.Soldiers[0].LeftTime) 
					/ Monster.Soldiers[0].CdTimeToFloat());
            }
            if (Player != null)
            {
                targetPlayerHp = (float)Player.HP / (float)Player.MaxHP; 
                foreach (var i in SkillGridTableManager)
                {
                    i.Model.Update();
                }
            }

            HpBar.value = Mathf.Lerp(HpBar.value, targetMonsterHp, Time.deltaTime * 5);
            PlayerHpBar.value = Mathf.Lerp(PlayerHpBar.value, targetPlayerHp, Time.deltaTime * 5);
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
			MonsterRoot.ActiveSelfObject (true);
            Monster = monster;
            lb_monsterName.text = Monster.Soldiers[0].Config.Name;
            var config = monster.Soldiers[0].Config;
            DataManagers.PlayerArmyManager.Singleton.SetJob(jobicon, config);
            this.lb_monster_lvl.text = string.Format(LanguageManager.Singleton["BATTLE_UI_MONSTER_LVL"], config.Level);
            DataManagers.PlayerArmyManager.Singleton.SetIcon(Monster_coin, config, DataManagers.TypeOfIcon.BattleMax);
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
            var st = per.State as BattleState; ;
            this.lb_title.text =st.GroupConfig .Name;
        }


        public void ShowBattleName(BattleConfig config)
        {
            lb_title.text = config.Name;
        }
        public void ShowDialog(ExcelConfig.BattleConfig battleConfig)
        {
            GameDebug.Log(battleConfig.Dialog);
            Per.State.Enable = false;
            UI.Windows.UIMessageBox.ShowMessage
			(
				battleConfig.Name, battleConfig.Dialog,
				() => { 
					Per.State.JoinAllItem();
					Per.ResetAllSkillCD();
					Per.State.Enable = true;  
				},
				() => {  ExitBattle(); }
			);
            //UITipDrawer.Singleton.DrawNotify(battleConfig.Dialog);
        }

		//NO success player select exit!!
		private void ExitBattle()
		{
			var state = this.Per.State as Combat.Battle.States.BattleState;
			state.End (new BattleResult{ Winner = Proto.ArmyCamp.Monster, Dead = false  });

		}


		public void OnMonsterDead(int monsterID)
		{
			if (this.OnMonsterDeadCallBack == null)
				return;
			OnMonsterDeadCallBack (monsterID);
		}

        public void OnAttack(Combat.Battle.Elements.DamageResult result,BattleArmy cur)
        {
			var sounds = new List<string> (){ 
			    "hit",
				"hit1",
				"hit2"
			};

			SoundManager.Singleton.PlaySound (GRandomer.RandomList (sounds));

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
				StopAllCoroutines ();
				StartCoroutine(DoetScale ());
                UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["LOST_HP"], result.Damage));
            }
        }

		private IEnumerator DoetScale()
		{
			TweenScale.Begin (this.MonsterRoot.gameObject, 0.1f, new Vector3 (2, 2, 2)).style = UITweener.Style.Once;
			yield return new  WaitForSeconds (0.1f);
			TweenScale.Begin (this.MonsterRoot.gameObject, 0.1f, new Vector3 (1, 1, 1)).style = UITweener.Style.Once;
		}

        private float targetPlayerHp =1f;
        private float targetMonsterHp = 1f;

        private bool _cancel = false;

        public bool Cancel
        {
            get { return _cancel; }
        }

		public Action<int> OnMonsterDeadCallBack;
    }
}