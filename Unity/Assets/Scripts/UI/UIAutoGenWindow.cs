using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// 自动生成UI
    /// 自动生成UI是为了将UI逻辑中对资源描述的部分分离，当资源发生变化的时候不用重新写逻辑代码
    /// 
    /// @author:xxp
    /// </summary>
    public abstract class UIAutoGenWindow : UIWindow
    {
        public override void OnCreate()
        {
            base.OnCreate();
            InitTemplate();
            InitModel();
        }

        public virtual void InitTemplate()
        { }

        public virtual void InitModel()
        { }
    }
}
