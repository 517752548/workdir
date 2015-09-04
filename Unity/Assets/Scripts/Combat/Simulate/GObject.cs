using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Simulate
{
    public abstract class GObject
    {
        public GObject(GControllor controllor) {
            Controllor = controllor;
            if (_index == int.MaxValue) _index = 0;
            Index = _index++;
        }
        private static int _index = 0;
        public int Index { private set; get; }

        public GControllor Controllor {  set; get; }

        public bool Enable { set; get; }

        public virtual void OnJoinState() { }

        public virtual void OnDestory() { }
    }
}
