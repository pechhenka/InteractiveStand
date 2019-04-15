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
            Loger.add("Стенд", "Запуск");
            SceneManager.LoadScene(1);
        }
    }
}