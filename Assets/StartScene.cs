using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stand
{
    public class StartScene : MonoBehaviour
    {
        void Awake()
        {
            Loger.add("Стенд", "Запуск");
            SceneManager.LoadScene(1);
        }
    }
}