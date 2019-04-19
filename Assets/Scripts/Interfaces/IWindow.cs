using UnityEngine;

namespace Stand
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class IWindow : MonoBehaviour
    {
        [HideInInspector] public CanvasGroup _CanvasGroup;
        [HideInInspector] public UIController _UIController;
        void Awake()
        {
            _CanvasGroup = GetComponent<CanvasGroup>();
            _UIController = GetComponentInParent<UIController>();
            SetActive(false);
        }

        abstract public void PrimaryFill();
        abstract public void Refill();
        abstract public void Fill();
        abstract public void Fill(int id);
        abstract public void Fill(GameObject gameObject);

        abstract public void ChooseClass(string Class);
        abstract public void ChooseDay(int id);

        abstract public void Merge(bool Status);

        public void SetActive(bool Open)
        {
            if (Open)
            {
                _CanvasGroup.alpha = 1;
                _CanvasGroup.interactable = true;
                _CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                _CanvasGroup.alpha = 0;
                _CanvasGroup.interactable = false;
                _CanvasGroup.blocksRaycasts = false;
            }
        }
    }
}