using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stand
{
    public class AnimController : MonoBehaviour
    {
        private Animation anim;
        private UIController UIC;

        private void Start()
        {
            anim = GetComponent<Animation>();
            UIC = GameObject.Find("[UI]").GetComponent<UIController>();
        }

        public void OnAboutTheAuthor()
        {
            UIC.LastTouch = Time.time;
            if (!anim.isPlaying)
                anim.Play("OnAboutTheAuthor");
        }

        public void OffAboutTheAuthor()
        {
            UIC.LastTouch = Time.time;
            if (!anim.isPlaying)
                anim.Play("OffAboutTheAuthor");
        }
    }
}