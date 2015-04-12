using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class GameDebug
    {
        public static void Log(object obj)
        {
            Debug.Log(obj);
        }
        public static void Log(string format, params object[] parms)
        {
            Debug.LogFormat(format, parms);
        }

        public static void LogError(object obj)
        {
            Debug.LogError(obj);
        }

        public static void LogWarning(object obj)
        {
            Debug.LogWarning(obj);
        }
        public static void LogException(Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
