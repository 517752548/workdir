using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Tools
{
    public class ResourcesManager : MonoBehaviour
    {
        private ResourcesManager()
        {
        }

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private static ResourcesManager _s;
        public static ResourcesManager Singleton
        {
            get
            {
                if (_s == null)
                {
                    var obj = new GameObject("__RESOURCES_LOADER__");
                    _s= obj.AddComponent<ResourcesManager>();
                }
                return _s;
            }
        }

        public string LastPath = string.Empty;
        
        public T LoadResources<T>(string resourcesPath) where T : Object
        {
            LastPath = resourcesPath;
            return Resources.Load<T>(resourcesPath);
        }

        public void LoadResourcesAnyc<T>(string path, Action<T> callBack) where T : Object
        {
            var request = Resources.LoadAsync<T>(path);
            StartCoroutine(LoadRes<T>(request, callBack));
        }

        private IEnumerator LoadRes<T>(ResourceRequest result, Action<T> callBack) where T : Object
        {
            while (!result.isDone)
            {
                yield return null;
            }
            callBack(result.asset as T);
        }
    }
}
