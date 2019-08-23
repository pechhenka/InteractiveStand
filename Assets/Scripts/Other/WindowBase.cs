using System;
using UnityEngine;

namespace Stand
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowBase : MonoBehaviour , IReceive<SignalSceneRaload>
    {
        [HideInInspector] public CanvasGroup _CanvasGroup;
        [HideInInspector] public UIController _UIController;
        private bool? OpenStatus = null;
        void Awake()
        {
            ProcessingSignals.Default.Add(this);
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
        abstract public void ChooseDay(DayOfWeek d);

        abstract public void Merge(bool Status);

        public void SetActive(bool Open)
        {
            if (OpenStatus != null && OpenStatus == Open)
                return;

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
            OpenStatus = Open;
        }

        void IReceive<SignalSceneRaload>.HandleSignal(SignalSceneRaload arg)
        {
            ProcessingSignals.Default.Remove(this);
        }
    }
}