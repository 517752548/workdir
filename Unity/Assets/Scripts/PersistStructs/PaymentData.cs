using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.PersistStructs
{
    public class PaymentData
    {
        public string BundleID { set; get; }
        public string SpriteName { set; get; }
        public string Des { set; get; }

        public int Reward { set; get; }
        public PaymentData()
        {
            BundleID = SpriteName = Des = string.Empty;
        }


        //public string 
    }
}
