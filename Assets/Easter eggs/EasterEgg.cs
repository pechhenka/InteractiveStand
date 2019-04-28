using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class EasterEgg : MonoBehaviour
    {
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
                Loger.Log("Пасхалка", "вход");
                anim.Play("OnAboutTheAuthor");
            }
        }

        public void OffAboutTheAuthor()
        {
            if (!anim.isPlaying)
            {
                Loger.Log("Пасхалка", "выход");
                anim.Play("OffAboutTheAuthor");
            }
        }
    }
}