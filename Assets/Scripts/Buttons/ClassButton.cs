using UnityEngine;

namespace Stand
{
    public class ClassButton : MonoBehaviour
    {
        UIController UIC;
        bool FirstEnable = false;

        void OnEnable()
        {
            if (FirstEnable)
                return;

            FirstEnable = true;
            UIC = GameObject.Find("[UI]").GetComponent<UIController>();
        }

        public void OnClick()
        {
            UIC.Lessons_ClassWindow_ChooseClass(transform.parent.name);
        }
    }
}