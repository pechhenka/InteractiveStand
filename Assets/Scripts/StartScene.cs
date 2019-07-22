using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stand
{
    /* Вызывается только при запуске программы
     */
    public class StartScene : MonoBehaviour
    {
        void Awake()
        {
            Loger.StartLoger();
            Loger.Log("Стенд", "Запуск");
            SceneManager.LoadScene(1);
        }
    }
}