using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.App
{
    public class GameState
    {
        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnTick() { }

        public virtual void OnTap(Vector2 pox) { }

		public virtual void OnPinch (PinchGesture gesture){}
    }
}
