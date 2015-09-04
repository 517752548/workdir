using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.States
{
    public interface IBattleRender
    {
        void ShowPlayer(Elements.BattleArmy player);
        void ShowMonster(Elements.BattleArmy monster);

        void SetPerception(BattlePerception per);
        int GetTapIndex();

        int ReleaseTapIndex();

        void ShowDialog(ExcelConfig.BattleConfig battleConfig);

        void ShowBattleName(ExcelConfig.BattleConfig config);
    }
}
