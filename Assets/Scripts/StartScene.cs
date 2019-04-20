using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stand
{
    /* Вызывается только при запуске программы
     * Он ничего не делает
     */
    public class StartScene : MonoBehaviour
    {
        void Awake()
        {
            Loger.Log("Стенд", "Запуск");
            SceneManager.LoadScene(1);
        }
    }
}