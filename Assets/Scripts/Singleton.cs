using UnityEngine;

namespace WorkBrew
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static System.Object _lock = new System.Object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var singleton = new GameObject("[SINGLETON] " + typeof(T));
                        _instance = singleton.AddComponent<T>();
                        DontDestroyOnLoad(singleton);
                    }

                    return _instance;
                }
            }
        }
    }
}