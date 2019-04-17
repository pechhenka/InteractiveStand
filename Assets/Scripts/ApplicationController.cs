using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stand
{
    public class ApplicationController : Singleton<ApplicationController>
    {

        public int Downtime = 0;
        public float LastTouch = 0f;
        public bool Clear = false;

        private Coroutine Cor;
        private bool Quit = false;

        void Awake()
        {
            LastTouch = Time.time;
        }

        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!Quit)
                {
                    Quit = true;
                    Loger.add("Стенд", "Выход с клавиатуры");
                    Application.Quit();
                }
            }

            if (Input.touchCount > 0 || Input.anyKey)
                LastTouch = Time.time;

            if ((Downtime > 0) && LastTouch + Downtime < Time.time)
            {
                Loger.add("Стенд", $"Возврат в нальное состояние&LastTouch:{LastTouch}");
                LastTouch = Time.time;
                SceneManager.LoadScene(1);
            }
        }

        void OnApplicationFocus(bool focus)
        {
            Loger.add("Стенд", "MonoBehaviour.OnApplicationFocus:" + focus);
        }

        void OnApplicationPause(bool pause)
        {
            Loger.add("Стенд", "MonoBehaviour.OnApplicationPause:" + pause);
        }

        void OnApplicationQuit()
        {
            if (!Quit)
                Loger.add("Стенд", "Закрыт");
        }
    }
}