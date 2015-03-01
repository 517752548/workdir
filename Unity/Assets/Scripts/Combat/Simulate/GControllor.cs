using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Simulate
{
    public abstract class GControllor
    {
        public GControllor(GPerception per)
        {
            Perception = per;
        }
        public GPerception Perception { private set; get; }

        public abstract GAction GetAction(GObject current);
    }
}
