using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Tools
{
    public class XSingleton<T>  where T : new()
    {
        private class InnerClass
        {
            public static T inst = new T();

            internal static void Reset()
            {
                inst = new T();
            }
        }

        public static T Singleton
        {
            get
            {
                return InnerClass.inst;
            }
        }

        public static void Reset()
        {
            InnerClass.Reset();
        }
    }
}
