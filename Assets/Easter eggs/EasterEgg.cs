using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Stand
{
    public class EasterEgg : MonoBehaviour
    {
        public Text BuildDated;

        private Animation anim;

        private void Start()
        {
            BuildDated.text = "build number: " + BuildInfo.BUILD_NUMBER + '\n' + "build dated: " + BuildInfo.BUILD_DATE;
            anim = GetComponent<Animation>();
        }

        public void OnAboutTheAuthor()
        {
            if (!anim.isPlaying)
            {
                Loger.Log("Пасхалка", new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("Вход",null)
                });
                anim.Play("OnAboutTheAuthor");
            }
        }

        public void OffAboutTheAuthor()
        {
            if (!anim.isPlaying)
            {
                Loger.Log("Пасхалка", new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("Выход",null)
                });
                anim.Play("OffAboutTheAuthor");
            }
        }
    }
}