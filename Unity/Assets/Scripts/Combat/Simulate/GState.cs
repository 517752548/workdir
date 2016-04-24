using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Simulate
{
    public abstract class GState
    {
        public GState() { _elements = new Dictionary<int, GObject>(); Enable = false; NeedEnd = false; }
        public GPerception Perception { protected set; get; }
        public bool Enable {  set; get; }
        public abstract void OnEnter();

        public abstract void OnExit();

		public void JoinAllItem()
		{
			while (_addTemp.Count > 0)
			{

				var item = _addTemp.Dequeue();
				if (_elements.ContainsKey(item.Index))
				{
					Debug.LogWarning("INDEX:"+item.Index+" HAD IN DICT!");
					continue;
				}
				item.Enable = true;
				item.OnJoinState();
				_elements.Add(item.Index, item);
			}
		}

        public virtual void OnTick()
        {
            if (!Enable) return;
			if (waitTime > 0) {
				if (waitTime > Time.time)
					return;
				waitTime = -1;
			}
			JoinAllItem ();
            foreach(var i in _elements)
            {
                if (!i.Value.Enable)
                {
                    //RemoveElement(i.Value);
                    _delTemp.Enqueue(i.Value);
                    continue;
                }

                i.Value.Controllor.GetAction(i.Value).DoAction();
            }

            while (_delTemp.Count > 0) 
            {
                var item = _delTemp.Dequeue();
                item.OnDestory();
                _elements.Remove(item.Index);
            }
        }

        private Dictionary<int, GObject> _elements;

        public T FindObjectByIndex<T>(int id) where T : GObject 
        {
            GObject item;
            if (_elements.TryGetValue(id, out item))
            {
                return item as T;
            }
            return null;
        }
        public GObject this[int index] { get { return FindObjectByIndex<GObject>(index); } }

        public delegate bool FindCondtion<T>(T el) where T : GObject;

        public void Each<T>(FindCondtion<T> cond) where T : GObject 
        {
            foreach (var i in _elements) 
            {
                var el = i.Value as T;
                if (el == null) continue;
                if (cond(el)) break;
            }
        }
        private Queue<GObject> _addTemp = new Queue<GObject>();
        private Queue<GObject> _delTemp = new Queue<GObject>();
        public void AddElement<T>(T el) where T : GObject {
            _addTemp.Enqueue(el);
        }
        public void RemoveElement<T>(T el) where T : GObject 
        {
            el.Enable = false;
        }

        public void Start()
        {
            if (Enable) return;
            Enable = true;
            OnEnter();
        }


        public bool NeedEnd { set; get; }

		private float waitTime = -1f;

		public void WaitForSeconds(float seconds)
		{
			if (waitTime > Time.time) {
				waitTime += seconds;
			} else {
				waitTime = Time.time + seconds;
			}
		}
    }
}
