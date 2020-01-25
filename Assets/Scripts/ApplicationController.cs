using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Stand
{
    public class ApplicationController : Singleton<ApplicationController>
    {

        public int DownTime = 0;
        public float LastTouch = 0f;
        public bool Clear = false;

        private bool Quit = false;

        void Awake()
        {
            LastTouch = Time.time;
        }

        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            {
                if (!Quit)
                {
                    Quit = true;
                    Loger.Log("Стенд", new List<KeyValuePair<string, string>>(){
                        new KeyValuePair<string, string>("Выход с клавиатуры", null)
                    });
                    Application.Quit();
                }
            }

            if (Input.touchCount > 0 || Input.anyKey)
                LastTouch = Time.time;

            if ((DownTime > 0) && LastTouch + DownTime < Time.time)
            {
                Loger.Log("Стенд", new List<KeyValuePair<string, string>>(){
                    new KeyValuePair<string, string>("Возврат в нальное состояние", null),
                    new KeyValuePair<string, string>("LastTouch", LastTouch.ToString())
                });
                ProcessingSignals.Default.Send(new SignalSceneRaload());
                LastTouch = Time.time;
                SceneManager.LoadScene(1);
            }
        }

        void OnApplicationFocus(bool focus)
        {
            Loger.Log("Стенд", new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("OnApplicationFocus", focus.ToString())
            });
        }

        void OnApplicationPause(bool pause)
        {
            Loger.Log("Стенд", new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("OnApplicationPause", pause.ToString())
            });
        }

        void OnApplicationQuit()
        {
            if (!Quit)
                Loger.Log("Стенд", new List<KeyValuePair<string, string>>(){
                    new KeyValuePair<string, string>("Закрыт", null)
                });
        }
    }
}