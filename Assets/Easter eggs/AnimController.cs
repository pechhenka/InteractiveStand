using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stand
{
    public class AnimController : MonoBehaviour
    {
        private Animation anim;

        private void Start()
        {
            anim = GetComponent<Animation>();
        }

        public void OnAboutTheAuthor()
        {
            if (!anim.isPlaying)
                anim.Play("OnAboutTheAuthor");
        }

        public void OffAboutTheAuthor()
        {
            if (!anim.isPlaying)
                anim.Play("OffAboutTheAuthor");
        }
    }
}