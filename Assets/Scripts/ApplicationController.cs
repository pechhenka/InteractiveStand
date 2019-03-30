using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stand
{
    public class ApplicationController : Singleton<ApplicationController>
    {

        public int Downtime = 0;
        public float LastTouch = 0f;
        public bool Clear = false;

        public float StartTimeInGame = -1000f;
        public float TimeInGame = 20f;
        public float StartTimePauseGame = -1000f;
        public float TimePauseGame = 60f;
        public bool GameTimeComplete = true;

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

        public void QuitGame()
        {
            StopCoroutine(Cor);
            Loger.add("Игра", "ручной выход");
            Loger.add("Пасхалка", "выход");
            SceneManager.LoadScene(1);
        }

        public void ActivateGame()
        {
            if (GameTimeComplete)
            {
                Loger.add("Игра", "Активирована");
                GameTimeComplete = false;
                StartCoroutine(TimePauseGameC());
                Cor = StartCoroutine(TimeInGameC());
                SceneManager.LoadScene(2);
            }
        }

        IEnumerator TimeInGameC()
        {
            StartTimeInGame = Time.time;
            yield return new WaitForSeconds(TimeInGame);
            Loger.add("Игра", "авто выход");
            Loger.add("Пасхалка", "выход");
            SceneManager.LoadScene(1);
        }

        IEnumerator TimePauseGameC()
        {
            StartTimePauseGame = Time.time;
            yield return new WaitForSeconds(TimePauseGame);
            GameTimeComplete = true;
        }
    }
}