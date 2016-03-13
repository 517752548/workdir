using Assets.Scripts.Combat.Simulate;
using ExcelConfig;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.States
{
    public class BattleState :GState
    {
        public BattleState(int battleGroupID, IBattleRender render, int battleIndex, List<int> solders, EndBattleCallBack callBack = null)
        {
            Render = render;
            CallBack = callBack;
            var perception = new BattlePerception(this);
            this.Perception = perception;
            var battle = new Elements.BattleEl(new Controllors.BattleControllor(this.Perception), battleGroupID);
            AddElement(battle);
            battle.BattleIndex = battleIndex;
            GroupConfig = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.BattleGroupConfig>(battleGroupID);
            PlayerSoldiers = solders;
            Render.SetPerception(Perception as BattlePerception);
        }

        public BattleGroupConfig GroupConfig { private set; get; }

        public IBattleRender Render { private set; get; }
        public List<int> PlayerSoldiers { get; internal set; }

        public EndBattleCallBack CallBack;

        public override void OnEnter()
        {
          
        }
        public override void OnExit()
        {
            //throw new NotImplementedException();
        }

        public void End(BattleResult result)
        {
            this.Enable = false;
            if (CallBack == null) return;
            CallBack(result);
        }


    }

    public class BattleResult
    {
        public ArmyCamp Winner{set;get;}
		public List<Proto.Item> DropList{set;get;}
		public int DropGold{ set; get; }
		public bool Dead{ set; get; }
		public List<int> MonsterIDS{ set; get; }
		public BattleResult()
		{
			DropGold = 0;
			Dead = false;
			DropList = new List<Item> ();
			Winner = ArmyCamp.Monster;
			MonsterIDS = new List<int> ();
		}
	}

    public delegate void EndBattleCallBack(BattleResult result);
}
