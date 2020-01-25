using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Stand
{
    /* Вызывается только при запуске программы
     */
    public class StartScene : MonoBehaviour
    {
        void Awake()
        {
            Loger.StartLoger();
            List<KeyValuePair<string, string>> StartSettings = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("Старт", null),
                new KeyValuePair<string, string>("Дата версии", BuildInfo.BUILD_DATE),
                new KeyValuePair<string, string>("Номер версии", BuildInfo.BUILD_NUMBER)
            };
            Loger.Log<StartScene>(StartSettings);
            Loger.Warning<StartScene>(StartSettings);
            Loger.Error<StartScene>(StartSettings);
            SceneManager.LoadScene(1);
        }
    }
}