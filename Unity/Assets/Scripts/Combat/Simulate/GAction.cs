using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Simulate
{
    public abstract class GAction
    {
        public GAction(GObject obj, GPerception per) 
        {
            Obj = obj;
            Perception = per;
        }
        public GObject Obj { private set; get; }
        public GPerception Perception { private set; get; }
        public abstract void DoAction();
        static GAction() {
            Empty = new EmptyAction();
        }
        public static EmptyAction Empty { private set; get; }
        public class EmptyAction:GAction
        {
            public EmptyAction() : base(null, null) { }

            public override void DoAction()
            {
                return;
            }
        }
    }
}
