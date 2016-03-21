using Assets.Scripts.Combat.Battle.Elements;
using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.States
{
	public class BattlePerception : GPerception
	{
		public BattlePerception (GState state)
			: base (state)
		{
		}

		public bool HaveDeadArmy ()
		{
			bool have = false;
			int count = 0;
			State.Each<BattleArmy> ((el) => {
				if (el.IsDead) {
					have = true;
					return true;
				}
				count++;
				return false;
			});
			if (count == 1)
				return true;
			return have;
		}

		public bool PlayerDead ()
		{
			bool have = true;
			State.Each<BattleArmy> ((el) => {
				if (!el.IsDead && el.Camp == Proto.ArmyCamp.Player) {
					have = false;
					return true;
				}
				return false;
			});
			return have;
		}

		public bool PlayerAddHp(int hp)
		{
			if (hp <= 0)
				return false;
			BattleArmy player =null;
			State.Each<BattleArmy> (el => {
				if(el.Camp == Proto.ArmyCamp.Player){
					player =el;
					return true;
				}
				return false;
			});
			if (player == null)
				return false;
			player.CalHp (hp);
			return true;
		}

		public BattleArmy GetEnemy (BattleArmy el)
		{
			BattleArmy enemy = null;
			State.Each<BattleArmy> ((item) => {
				if (item.IsDead)
					return false;
				if (item.Camp != el.Camp) {
					enemy = item;
					return true;
				}
				return false;
			});
			return enemy;
		}

		internal BattleEl GetBattle ()
		{
			BattleEl el = null;
			State.Each<BattleEl> ((item) => {
				el = item;
				return true;
			});
			return el;
		}

		public void ChangePlayerControllor (bool auto)
		{
			BattleArmy player = null;
			State.Each<BattleArmy> ((t) => {
				if (t.Army.Camp == Proto.ArmyCamp.Player) {
					player = t;
					return true;
				}
				return false;
			});
			if (player == null)
				return;
			
			GControllor controllor;
			if (auto)
				controllor = new Controllors.ArmyControllor (this);
			else
				controllor = new Controllors.ArmyPlayerControllor (this);

			player.Controllor = controllor;
		}

		public void ResetAllSkillCD ()
		{
			State.Each<BattleArmy> (t => {
				foreach (var s in t.Soldiers) {
					s.AttackCdTime = Time.time;
				}
				return false;
			});
		}

		public void ClearPlayerCD()
		{
			State.Each<BattleArmy> (t => {
				if(t.Camp != Proto.ArmyCamp.Player)return false;
				foreach (var s in t.Soldiers) {
					s.AttackCdTime = 0;//Time.time;
				}
				return false;
			});
		}
	}
}
