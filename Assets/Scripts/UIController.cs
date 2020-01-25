using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class UIController : MonoBehaviour
    {
        [Header("Colors")]
        public Color LowGray;
        public Color Gray;
        public Color VeryGray;

        [Header("BlindMode")]
        public float AnimTimeBlind = 1f;
        public Camera cam;
        public Material GrayScale;
        public AnimationCurve ScaleCam;
        public AnimationCurve GrayScaleAnim;

        private readonly float NoBlind = 5f;
        private readonly float YesBlind = 4.3f;
        private float StartAnimBlind = -10f;

        [Header("Buttons")]
        public GameObject CallsButton;
        public GameObject LessonsButton;
        public GameObject ExtraButton;
        public GameObject ChangeCallsWindowButton;
        public GameObject ChangeLessonsWindowButton;

        [Header("Mains")]
        public WindowBase MainStaticSchedulesWindow;
        public WindowBase MainChangesSchedulesWindow;
        private WindowBase MainCurrentWindow;

        [Header("Calls")]
        public WindowBase CallsWindow;

        [Header("Lessons")]
        public WindowBase LessonsWindow;

        [Header("Lessons_Class")]
        public WindowBase Lessons_ClassWindow;

        [Header("Extra")]
        public WindowBase ExtraWindow;

        [Header("ChangeCallsWindow")]
        public WindowBase ChangeCallsWindow;

        [Header("ChangeLessonsWindow")]
        public WindowBase ChangeLessonsWindow;

        [Header("TimePanel")]
        public WindowBase TimePanelWindow;

        [Header("TimeLine")]
        public WindowBase TimeLineWindow;

        bool BlindMode = false;

        void Start()
        {
            MainChangesSchedulesWindow.SetActive(false);
            MainStaticSchedulesWindow.SetActive(false);

            if (Data.Instance.CurrentManifest.SupportChangesSchedules)
                MainCurrentWindow = MainChangesSchedulesWindow;
            else
                MainCurrentWindow = MainStaticSchedulesWindow;

            CallsButton = MainCurrentWindow.transform.Find("Button (Calls)").gameObject;
            LessonsButton = MainCurrentWindow.transform.Find("Button (Lessons)").gameObject;
            ExtraButton = MainCurrentWindow.transform.Find("Button (Extra)").gameObject;

            HideAll();
            MainCurrentWindow.SetActive(true);
            TimePanelWindow.PrimaryFill();
            TimePanelWindow.SetActive(true);
            TimeLineWindow.SetActive(true);
            LessonsWindow.PrimaryFill();
            CallsWindow.PrimaryFill();
            GrayScale.SetFloat("_EffectAmount", 0f);
        }

        void FixedUpdate()
        {
            TimePanelWindow.Fill();
            TimeLineWindow.Fill();

            if (StartAnimBlind + AnimTimeBlind > Time.time)
            {
                float t = (Time.time - StartAnimBlind) / AnimTimeBlind;

                if (t > 0.8f)
                    ApplicationController.Instance.Clear = false;
                else
                    ApplicationController.Instance.Clear = true;

                if (BlindMode)
                {
                    cam.orthographicSize = Mathf.Lerp(NoBlind, YesBlind, ScaleCam.Evaluate(t));
                    GrayScale.SetFloat("_EffectAmount", GrayScaleAnim.Evaluate(t));

                }
                else
                {
                    cam.orthographicSize = Mathf.Lerp(YesBlind, NoBlind, ScaleCam.Evaluate(t));
                    GrayScale.SetFloat("_EffectAmount", 1 - GrayScaleAnim.Evaluate(t));
                }
            }

            if (Data.Instance.CurrentManifest.HideChangeButtonsIfOutdated)
            {
                CallsButton.SetActive(Data.Instance.CallsMatrix != null);
                LessonsButton.SetActive(Data.Instance.LessonsMatrix != null);
                ExtraButton.SetActive(Data.Instance.ExtraMatrix != null);
                ChangeCallsWindowButton.SetActive((ChangeCallsWindow as ChangeCallsWindow).GetChanges());
                ChangeLessonsWindowButton.SetActive((ChangeLessonsWindow as ChangeLessonsWindow).GetChanges());
            }
            else
            {
                CallsButton.SetActive(true);
                LessonsButton.SetActive(true);
                ExtraButton.SetActive(true);
                ChangeCallsWindowButton.SetActive(true);
                ChangeLessonsWindowButton.SetActive(true);
            }
        }

        public void Lessons_ClassWindow_ChooseClass(string Class)
        {
            Lessons_ClassWindow.SetActive(true);
            LessonsWindow.SetActive(false);
            TimePanelWindow.Merge(true);
            TimePanelWindow.SetActive(true);
            Lessons_ClassWindow.ChooseClass(Class);
        }

        public void MergeTimePanel(bool Status)
        {
            TimePanelWindow.Merge(Status);
        }

        public void OpenCallsWindow(bool Open)
        {
            HideAll();
            CallsWindow.SetActive(Open);
            TimePanelWindow.SetActive(true);
            MainCurrentWindow.SetActive(!Open);
            TimePanelWindow.Merge(Open);
            Loger.Log("(UIC)Окно звонков", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Действие",Open ? "открыли" : "закрыли")
            });
        }

        public void OpenLessonsWindow(bool Open)
        {
            HideAll();
            LessonsWindow.SetActive(Open);
            TimePanelWindow.SetActive(!Open);
            MainCurrentWindow.SetActive(!Open);
            Loger.Log("(UIC)Окно уроков", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Действие",Open ? "открыли" : "закрыли")
            });
        }

        public void OpenLessons_ClassWindow(bool Open)
        {
            HideAll();
            LessonsWindow.SetActive(true);
            TimePanelWindow.SetActive(false);
            MainCurrentWindow.SetActive(false);
            Loger.Log("(UIC)Окно занятий", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Действие",Open ? "открыли" : "закрыли")
            });
        }

        public void OpenExtraWindow(bool Open)
        {
            HideAll();
            ExtraWindow.PrimaryFill();
            TimePanelWindow.Merge(Open);

            MainCurrentWindow.SetActive(!Open);
            ExtraWindow.SetActive(Open);
            TimePanelWindow.SetActive(true);
            Loger.Log("(UIC)Окно доп.секций", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Действие",Open ? "открыли" : "закрыли")
            });
        }

        public void OpenChangeCallsWindow(bool Open)
        {
            HideAll();
            ChangeCallsWindow.PrimaryFill();
            TimePanelWindow.Merge(Open);

            MainCurrentWindow.SetActive(!Open);
            ChangeCallsWindow.SetActive(Open);
            TimePanelWindow.SetActive(true);
            Loger.Log("(UIC)Окно изменений звонков", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Действие",Open ? "открыли" : "закрыли")
            });
        }
        public void OpenChangeLessonsWindow(bool Open)
        {
            HideAll();
            ChangeLessonsWindow.PrimaryFill();
            TimePanelWindow.Merge(Open);

            MainCurrentWindow.SetActive(!Open);
            ChangeLessonsWindow.SetActive(Open);
            TimePanelWindow.SetActive(true);
            Loger.Log("(UIC)Окно изменений уроков", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Действие",Open ? "открыли" : "закрыли")
            });
        }
        void HideAll()
        {
            MainCurrentWindow.SetActive(false);
            CallsWindow.SetActive(false);
            LessonsWindow.SetActive(false);
            ExtraWindow.SetActive(false);
            TimePanelWindow.SetActive(false);
            Lessons_ClassWindow.SetActive(false);
            ChangeCallsWindow.SetActive(false);
            TimePanelWindow.Merge(false);
        }

        public void OnClickBlindMode()
        {
            BlindMode = !BlindMode;
            Loger.Log("(UIC)BlindMode", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Действие",BlindMode ? "Включён" : "Выключен")
            });
            StartAnimBlind = Time.time;
        }
    }
}