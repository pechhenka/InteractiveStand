using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stand
{
    public class ApplicationController : Singleton<ApplicationController>
    {

        public int Downtime = 0;
        public float LastTouch = 0f;

        void Awake()
        {
            LastTouch = Time.time;
        }

        void FixedUpdate()
        {
            if (Input.touchCount > 0 || Input.anyKey)
                LastTouch = Time.time;

            if ((Downtime > 0) && LastTouch + Downtime < Time.time)
            {
                LastTouch = Time.time;
                SceneManager.LoadScene(0);
            }
        }
    }
}