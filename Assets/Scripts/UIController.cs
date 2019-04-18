using System;
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

        [Header("Prefabs")]
        public GameObject InformationBlockPrefab;
        public GameObject Dot;

        [Header("BlindMode")]
        public float AnimTimeBlind = 1f;
        public Camera cam;
        public Material GrayScale;
        public AnimationCurve ScaleCam;
        public AnimationCurve GrayScaleAnim;

        private float NoBlind = 5f;
        private float YesBlind = 4.3f;
        private float StartAnimBlind = -10f;

        [Header("Main")]
        public GameObject Main;

        [Header("Calls")]
        public IWindow CallsWindow;

        [Header("Lessons")]
        public IWindow LessonsWindow;

        [Header("Lessons_Class")]
        public IWindow Lessons_ClassWindow;

        [Header("Extra")]
        public GameObject ExtraClasses;
        public Text HeadlineExtraClasses;
        public GameObject InformationsBlocks;
        public GameObject Dots;
        public Vector3 EndPos;
        public Vector3 EndScale;
        public AnimationCurve pos_IB;
        public AnimationCurve Scale_IB;
        public float AnimationTime = 1f;

        private float StartAnimation = -10f;
        private Vector3 StartPos = new Vector3(0, 227.5f, 0);
        private Vector3 StartScale = new Vector3(1, 1, 1);

        private int CurrentExtraDay = 0;
        private int CurrentExtraBlock = 0;
        private int LastExtraBlock = 0;
        private int LengthExtraBlocks = 0;
        private bool ActivateNextRightBlock = false;
        private bool EndAnimation = false;

        [Header("TimePanel")]
        public IWindow TimePanelWindow;

        [Header("TimeLine")]
        public IWindow TimeLineWindow;

        bool BlindMode = false;

        void Start()
        {
            TimePanelWindow.PrimaryFill();
            HideAll();
            Main.SetActive(true);
            TimePanelWindow.SetActive(true);
            LessonsWindow.PrimaryFill();
            CallsWindow.PrimaryFill();
            GrayScale.SetFloat("_EffectAmount", 0f);
        }

        void FixedUpdate()
        {
            TimePanelWindow.Fill();

            if (StartAnimation + AnimationTime > Time.time)
            {
                RectTransform LastEIB = InformationsBlocks.transform.GetChild(LastExtraBlock).GetComponent<RectTransform>();
                RectTransform CurrentEIB = InformationsBlocks.transform.GetChild(CurrentExtraBlock).GetComponent<RectTransform>();

                float t = (Time.time - StartAnimation) / AnimationTime;
                if (ActivateNextRightBlock)
                {
                    LastEIB.anchoredPosition = Vector3.Lerp(StartPos, EndPos.NegativeX(), pos_IB.Evaluate(t));
                    CurrentEIB.anchoredPosition = Vector3.Lerp(EndPos, StartPos, pos_IB.Evaluate(t));
                }
                else
                {
                    LastEIB.anchoredPosition = Vector3.Lerp(StartPos, EndPos, pos_IB.Evaluate(t));
                    CurrentEIB.anchoredPosition = Vector3.Lerp(EndPos.NegativeX(), StartPos, pos_IB.Evaluate(t));
                }
                LastEIB.localScale = Vector3.Lerp(EndScale, StartScale, Scale_IB.Evaluate(1 - t));
                CurrentEIB.localScale = Vector3.Lerp(EndScale, StartScale, Scale_IB.Evaluate(t));
                EndAnimation = true;
            }
            else if (EndAnimation)
            {
                InformationsBlocks.transform.GetChild(LastExtraBlock).GetComponent<CanvasGroup>().alpha = 0f;
                RectTransform CurrentRT = InformationsBlocks.transform.GetChild(CurrentExtraBlock).GetComponent<RectTransform>();
                CurrentRT.GetComponent<CanvasGroup>().alpha = 1f;
                CurrentRT.anchoredPosition = StartPos;
                CurrentRT.localScale = StartScale;
                EndAnimation = false;
            }

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
        }

        public void Lessons_ClassWindow_ChooseClass(string ClassID)
        {
            Lessons_ClassWindow.SetActive(true);
            LessonsWindow.SetActive(false);
            TimePanelWindow.Merge(true);
            TimePanelWindow.SetActive(true);
            Lessons_ClassWindow.ChooseClass(ClassID);
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
            Main.SetActive(!Open);
            TimePanelWindow.Merge(Open);
            Loger.add("Окно звонков", Open ? "открыли" : "закрыли");
        }

        public void OpenLessonsWindow(bool Open)
        {
            HideAll();
            LessonsWindow.SetActive(Open);
            TimePanelWindow.SetActive(!Open);
            Main.SetActive(!Open);
            Loger.add("Окно уроков", Open ? "открыли" : "закрыли");
        }

        public void OpenLessons_ClassWindow(bool Open)
        {
            HideAll();
            LessonsWindow.SetActive(true);
            TimePanelWindow.SetActive(false);
            Main.SetActive(false);
            Loger.add("Окно занятий", Open ? "открыли" : "закрыли");
        }

        public void OnExtraClasses(bool Open)
        {
            HideAll();
            ExtraClasses.SetActive(Open);
            TimePanelWindow.SetActive(true);
            Main.SetActive(!Open);
            TimePanelWindow.Merge(Open);
            HeadlineExtraClasses.text = DateTime.Now.DayOfWeek.ConvertToString(false);
            CurrentExtraDay = DateTime.Now.DayOfWeek.Normalising();
            Loger.add("Окно доп.секций", Open ? "открыли" : "закрыли");
            if (Open)
                FillExtraClasses();
        }

        void FillExtraClasses()
        {
            Loger.add($"Окно доп.секций&IdDay:{CurrentExtraDay}", "открыли");


            foreach (Transform child in InformationsBlocks.transform)
                Destroy(child.gameObject);
            foreach (Transform child in Dots.transform)
                Destroy(child.gameObject);

            CurrentExtraBlock = 0;
            LengthExtraBlocks = Data.Instance.ExtraClassesMatrix.Count;

            int j = CurrentExtraDay * 4;
            GameObject go = null;
            int i = 1;
            for (; i < LengthExtraBlocks; i++)
            {
                if (Data.Instance.ExtraClassesMatrix[i][j] == "")
                {
                    if (i == 1)
                    {
                        go = Instantiate(InformationBlockPrefab, InformationsBlocks.transform);
                        go.transform.Find("CourseName").GetComponent<Text>().text = "нет данных";
                        go.transform.Find("Classes").GetComponent<Text>().text = "";
                        go.transform.Find("Time").GetComponent<Text>().text = "";
                        go.transform.Find("Сlassroom").GetComponent<Text>().text = "";
                    }
                    break;
                }

                if (i == 1)
                    Instantiate(Dot, Dots.transform).GetComponent<Image>().color = Color.white;
                else
                    Instantiate(Dot, Dots.transform);

                go = Instantiate(InformationBlockPrefab, InformationsBlocks.transform);
                go.transform.Find("CourseName").GetComponent<Text>().text = Data.Instance.ExtraClassesMatrix[i][j];
                go.transform.Find("Classes").GetComponent<Text>().text = "Классы: " + Data.Instance.ExtraClassesMatrix[i][j + 1];
                go.transform.Find("Time").GetComponent<Text>().text = "Время: " + Data.Instance.ExtraClassesMatrix[i][j + 2];
                go.transform.Find("Сlassroom").GetComponent<Text>().text = "Кабинет: " + Data.Instance.ExtraClassesMatrix[i][j + 3];

                if (i != 1)
                    go.GetComponent<CanvasGroup>().alpha = 0f;
            }
            LengthExtraBlocks = i - 1;
        }

        public void BackBlockExtraClasses()
        {
            if (EndAnimation)
                return;

            EndAnimation = false;
            LastExtraBlock = CurrentExtraBlock;
            if (CurrentExtraBlock - 1 < 0)
                CurrentExtraBlock = LengthExtraBlocks - 1;
            else
                CurrentExtraBlock--;
            ActivateNextRightBlock = false;

            MoveBlockExtraClasses();
        }

        public void NextBlockExtraClasses()
        {
            if (EndAnimation)
                return;

            EndAnimation = false;
            LastExtraBlock = CurrentExtraBlock;
            if (CurrentExtraBlock + 1 >= LengthExtraBlocks)
                CurrentExtraBlock = 0;
            else
                CurrentExtraBlock++;
            ActivateNextRightBlock = true;

            MoveBlockExtraClasses();
        }

        void MoveBlockExtraClasses()
        {
            InformationsBlocks.transform.GetChild(CurrentExtraBlock).GetComponent<CanvasGroup>().alpha = 1f;
            Dots.transform.GetChild(LastExtraBlock).GetComponent<Image>().color = LowGray;
            Dots.transform.GetChild(CurrentExtraBlock).GetComponent<Image>().color = Color.white;
            StartAnimation = Time.time;

            RectTransform CurrentEIB = InformationsBlocks.transform.GetChild(CurrentExtraBlock).GetComponent<RectTransform>();
            if (ActivateNextRightBlock)
                CurrentEIB.anchoredPosition = EndPos;
            else
                CurrentEIB.anchoredPosition = EndPos.NegativeX();
            CurrentEIB.localScale = EndScale;
        }

        public void ClickExtraClassDay(int id)
        {
            if (id != CurrentExtraDay)
            {
                CurrentExtraDay = id;
                HeadlineExtraClasses.text = ((DayOfWeek)(id + 1)).ConvertToString(false);
                FillExtraClasses();
            }
            else
            {
                Loger.add($"Окно доп.секций&IdDay:{CurrentExtraDay}", "нервное касание");
            }
        }

        void HideAll()
        {
            Main.SetActive(false);
            CallsWindow.SetActive(false);
            LessonsWindow.SetActive(false);
            ExtraClasses.SetActive(false);
            TimePanelWindow.SetActive(false);
            Lessons_ClassWindow.SetActive(false);
            TimeLineWindow.SetActive(false);
            TimePanelWindow.Merge(false);
        }

        public void OnClickBlindMode()
        {
            BlindMode = !BlindMode;
            Loger.add("BlindMode", BlindMode ? "включён" : "выключен");
            StartAnimBlind = Time.time;
        }
    }
}