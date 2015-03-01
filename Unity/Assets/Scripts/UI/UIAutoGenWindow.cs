using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI
{
    public class UIAutoGenWindow : UIWindow
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
