using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Simulate
{
    public abstract class GPerception
    {
        public GState State { private set; get; }

        public GPerception(GState state) {
            State = state;
        }

        


    }
}
