using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UPEHFHook.Patches
{
    public class CoroutineHelper : MonoBehaviour
    {
        private static CoroutineHelper _instance;

        public static CoroutineHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject helperObject = new GameObject("CoroutineHelper");
                    _instance = helperObject.AddComponent<CoroutineHelper>();
                    GameObject.DontDestroyOnLoad(helperObject);
                }
                return _instance;
            }
        }
    }
}