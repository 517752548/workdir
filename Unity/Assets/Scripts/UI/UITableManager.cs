﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI
{
    public class UITableItem
    {
        public Transform Root { private set; get; }
        public virtual void OnCreate(Transform root)
        {
            Root = root;
        }
        public T FindChild<T>(string name) where T:Component
        {
            return this.Root.FindChild<T>(name);
        }
    }
    public class UITableManager<T> : IEnumerable<T> where T : UITableItem, new()
    {
        private int _count = 0;

        public bool Cached { set; get; }
        public int Count
        {
            get { return _count; }
            set
            {
                if (templet == null)
                    throw new Exception("Try to manage a not init table.");
                for (var i = 0; i < _items.Count; i++)
                {
                    if (_items[i].Root.gameObject.activeSelf) continue;
                    _items[i].Root.ActiveSelfObject(true);
                }
                if (_items.Count != value)
                {

                    if (_items.Count > value)
                    {
                        #region add
                        Queue<T> dels = new Queue<T>();
                        for (var i = value; i < _items.Count; i++)
                        {
                            if (Cached)
                            {
                                if (_items[i].Root.gameObject.activeSelf)
                                {
                                    _items[i].Root.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                dels.Enqueue(_items[i]);
                            }
                        }
                        while (dels.Count > 0)
                        {
                            var item = dels.Dequeue();
                            GameObject.Destroy(item.Root.gameObject);
                            _items.Remove(item);
                        }
                        #endregion
                    }
                    else
                    {
                        var count = _items.Count;
                        for (var i = count; i < value; i++)
                        {
                            var item = new T();
                            var obj = GameObject.Instantiate(templet.gameObject) as GameObject;
                            obj.name = string.Format("{0}_{1:0000}", templet.gameObject.name, i);
                            obj.transform.parent = templet.parent;
                            obj.transform.localScale = Vector3.one;
                            obj.SetActive(true);
                            item.OnCreate(obj.transform);
                            _items.Add(item);
                        }
                    }

                }
                _count = value;
                RepositionLayout();
            }
        }

        public void RepositionLayout()
        {
            if (currentTable != null)
                currentTable.repositionNow = true;
            if (currentGrid != null)
                currentGrid.repositionNow = true;
            //throw new NotImplementedException();
        }

        private List<T> _items = new List<T>();

        private Transform templet;
        public void Init(Transform root)
        {
            if (root.childCount > 0)
                templet = root.GetChild(0);
            else
                throw new Exception("Can't init table from a empty root!");
            templet.ActiveSelfObject(false);
        }

        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < Count)
                    return _items[index];
                else return null;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _items.Take(_count).GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._items.Take(_count).GetEnumerator();
        }

        private UIGrid currentGrid;
        private UITable currentTable;
        internal void InitFromGrid(UIGrid grid)
        {
            currentGrid = grid;
            Init(grid.transform);
        }

        public void InitFromTable(UITable table)
        {
            currentTable = table;
            Init(table.transform);
        }
    }
    public abstract class TableItemTemplate
    {
        public virtual void Init(UITableItem item)
        {
            Item = item;
            InitTemplate();
        }
        private UITableItem Item { set; get; }
        public T FindChild<T>(string name) where T : Component
        {
            return this.Item.FindChild<T>(name);
        }

        public abstract void InitTemplate();
    }
    public abstract class TableItemModel<UITemplate> where UITemplate : TableItemTemplate, new()
    {
        public virtual void Init(UITemplate template, UITableItem item)
        {
            Template = template;
            Item = item;
            InitModel();
        }
        public UITemplate Template { private set; get; }
        public UITableItem Item { private set; get; }
        public abstract void InitModel();
    }
    public class AutoGenTableItem<UITemplate, UIModel> : UITableItem
        where UITemplate : TableItemTemplate, new()
        where UIModel : TableItemModel<UITemplate>, new()
    {
        public AutoGenTableItem()
        { }

        public UITemplate Template { private set; get; }
        public UIModel Model { private set; get; }

        public override void OnCreate(Transform root)
        {
            base.OnCreate(root);
            Template = new UITemplate();
            Template.Init(this);
            Model = new UIModel();
            Model.Init(Template, this);
        }
    }
}
