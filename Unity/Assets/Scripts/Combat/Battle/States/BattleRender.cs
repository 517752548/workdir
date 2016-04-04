using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelConfig;
using Assets.Scripts.Combat.Battle.Elements;

namespace Assets.Scripts.Combat.Battle.States
{
    public interface IBattleRender
    {
        void ShowPlayer(Elements.BattleArmy player);
        void ShowMonster(Elements.BattleArmy monster);

        void SetPerception(BattlePerception per);
        int  GetTapIndex();

        int ReleaseTapIndex();

        void ShowDialog(BattleConfig battleConfig);

        void ShowBattleName(BattleConfig config);

        void OnAttack(DamageResult result,BattleArmy cur);

        bool Cancel { get; }

		void OnMonsterDead (int monsterID);
    }
}
