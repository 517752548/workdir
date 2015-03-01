using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Appliaction
{
    public class GameState
    {
        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnTick() { }
    }
}
