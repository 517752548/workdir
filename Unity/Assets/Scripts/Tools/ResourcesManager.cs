using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Tools
{
    class ResourcesManager : Tools.XSingleton<ResourcesManager>
    {
        public T LoadResources<T>(string resourcesPath) where T: Object
        {
            return Resources.Load<T>(resourcesPath);
        }
    }
}
