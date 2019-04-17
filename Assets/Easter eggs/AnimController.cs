using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class AnimController : MonoBehaviour
    {
        public Text TimerOnGame;
        public Text BuildDated;

        private Animation anim;

        private void Start()
        {
            BuildDated.text = "build dated: " + BuildInfo.BUILD_DATE;
            anim = GetComponent<Animation>();
        }

        public void OnAboutTheAuthor()
        {
            if (!anim.isPlaying)
            {
                Loger.add("Пасхалка", "вход");
                anim.Play("OnAboutTheAuthor");
            }
        }

        public void OffAboutTheAuthor()
        {
            if (!anim.isPlaying)
            {
                Loger.add("Пасхалка", "выход");
                anim.Play("OffAboutTheAuthor");
            }
        }
    }
}