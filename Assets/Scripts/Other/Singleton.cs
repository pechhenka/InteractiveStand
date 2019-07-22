using UnityEngine;


namespace Stand
{
    /* !Только для наследования!
     * При унаследовании данного класса ваш класс будет существовать в единственном экземляре
     * и будет достпуен из любой точки программы
     */ 
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            var singleton = new GameObject("[SINGLETON] " + typeof(T));
                            _instance = singleton.AddComponent<T>();
                            DontDestroyOnLoad(singleton);
                        }

                    }

                    return _instance;
                }
            }
        }
    }
}