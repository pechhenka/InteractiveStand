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
        public GameObject ClassesButton;
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

        [Header("Timetable")]
        public GameObject Timetable;
        public Text Monday;
        public Text Tuesday;
        public Text Saturday;

        [Header("LessonSchedule")]
        public GameObject LessonSchedule;
        public GameObject[] ClassButtons;

        [Header("WeeklySchedule")]
        public GameObject WeeklySchedule;
        public Text HeadlineWeeklySchedule;
        public Text WeeklyLessons;
        public Text WeeklyClassrooms;

        private int CurrentWeeklyDay = 0;
        private int CurrentWeeklyClass = 0;

        [Header("ExtraClasses")]
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

        [Header("DatePanel")]
        public GameObject DatePanel;
        public Text TimeUI;
        public Text DateUI;
        public Image DatePanelImage;

        bool BlindMode = false;

        void Awake()
        {
            string day = DateTime.Now.DayOfWeek.ConvertToString();
            TimeUI.text = DateTime.Now.ToString("HH:mm ") + day;
            DateUI.text = DateTime.Now.ToString("dd.MM.yyyy");
            HideAll();
            Main.SetActive(true);
            DatePanel.SetActive(true);
            GenerateClassesButtons();
            FillTimetable();
            GrayScale.SetFloat("_EffectAmount", 0f);
        }

        void FixedUpdate()
        {
            string day = DateTime.Now.DayOfWeek.ConvertToString();
            TimeUI.text = DateTime.Now.ToString("HH:mm ") + day;
            DateUI.text = DateTime.Now.ToString("dd.MM.yyyy");

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

        void FillTimetable()
        {
            Monday.text = "";
            Tuesday.text = "";
            Saturday.text = "";

            foreach (string[] s in Stock.Instance.TimetableMatrix)
            {
                Monday.text += s[0] + '\n';
                Tuesday.text += s[1] + '\n';
                Saturday.text += s[2] + '\n';
            }
        }

        public void ClickClass(GameObject ClassButton)
        {
            if (ClassButton.name == "Back")
            {
                OnLessonSchedule(true);
                return;
            }
            HideAll();
            WeeklySchedule.SetActive(true);
            DatePanel.SetActive(true);
            int number = int.Parse(ClassButton.name);
            HeadlineWeeklySchedule.text = DateTime.Now.DayOfWeek.ConvertToString(false) + " " + Stock.Instance.LessonScheduleMatrix[2][number * 4 + 3];
            DatePanelImage.color = VeryGray;
            CurrentWeeklyDay = DateTime.Now.DayOfWeek.Normalising();
            CurrentWeeklyClass = number;
            RefreshWeeklySchedule();
        }

        public void ClickClassDay(int id)
        {
            CurrentWeeklyDay = id;
            string NameClass = HeadlineWeeklySchedule.text.Split()[1];
            HeadlineWeeklySchedule.text = ((DayOfWeek)(id + 1)).ConvertToString(false) + " " + NameClass;
            RefreshWeeklySchedule();
        }

        void RefreshWeeklySchedule()
        {
            WeeklyClassrooms.text = "";
            WeeklyLessons.text = "";

            int CWD = CurrentWeeklyDay * 16 + 3;
            int CWC = CurrentWeeklyClass * 4;
            bool NullLesson = true;

            for (int i = 0; i < 8; i++)
            {
                if (Stock.Instance.LessonScheduleMatrix[CWD + i * 2][CWC + 3] == "")
                {
                    if (NullLesson)
                    {
                        WeeklyLessons.text += "--" + '\n';
                        WeeklyClassrooms.text += "--" + '\n';
                        continue;
                    }
                    else
                        break;
                }
                else
                    NullLesson = false;
                WeeklyLessons.text += Stock.Instance.LessonScheduleMatrix[CWD + i * 2][CWC + 3] + '\n';

                string Classrooms = "";
                if (Stock.Instance.LessonScheduleMatrix[CWD + i * 2 + 1][CWC + 4] != "")
                {
                    Classrooms += Stock.Instance.LessonScheduleMatrix[CWD + i * 2 + 1][CWC + 4];
                    if (Stock.Instance.LessonScheduleMatrix[CWD + i * 2 + 1][CWC + 6] != "")
                        Classrooms += "/" + Stock.Instance.LessonScheduleMatrix[CWD + i * 2 + 1][CWC + 6];
                }
                WeeklyClassrooms.text += Classrooms + '\n';
            }
        }

        public void OnTimetable(bool Open)
        {
            HideAll();
            Timetable.SetActive(Open);
            DatePanel.SetActive(true);
            Main.SetActive(!Open);
            DatePanelImage.color = Open ? VeryGray : Gray;
        }

        public void OnLessonSchedule(bool Open)
        {
            HideAll();
            LessonSchedule.SetActive(Open);
            DatePanel.SetActive(!Open);
            Main.SetActive(!Open);
        }

        public void OnExtraClasses(bool Open)
        {
            HideAll();
            ExtraClasses.SetActive(Open);
            DatePanel.SetActive(true);
            Main.SetActive(!Open);
            DatePanelImage.color = Open ? VeryGray : Gray;
            HeadlineExtraClasses.text = DateTime.Now.DayOfWeek.ConvertToString(false);
            CurrentExtraDay = DateTime.Now.DayOfWeek.Normalising();
            FillExtraClasses();
        }

        void FillExtraClasses()
        {
            foreach (Transform child in InformationsBlocks.transform)
                Destroy(child.gameObject);
            foreach (Transform child in Dots.transform)
                Destroy(child.gameObject);

            CurrentExtraBlock = 0;
            LengthExtraBlocks = Stock.Instance.ExtraClassesMatrix.Count;

            int j = CurrentExtraDay * 4;
            GameObject go = null;
            int i = 1;
            for (; i < LengthExtraBlocks; i++)
            {
                j = CurrentExtraDay * 4;

                if (Stock.Instance.ExtraClassesMatrix[i][j] == "")
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
                go.transform.Find("CourseName").GetComponent<Text>().text = Stock.Instance.ExtraClassesMatrix[i][j];
                go.transform.Find("Classes").GetComponent<Text>().text = "Классы: " + Stock.Instance.ExtraClassesMatrix[i][j + 1];
                go.transform.Find("Time").GetComponent<Text>().text = "Время: " + Stock.Instance.ExtraClassesMatrix[i][j + 2];
                go.transform.Find("Сlassroom").GetComponent<Text>().text = "Кабинет: " + Stock.Instance.ExtraClassesMatrix[i][j + 3];

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
            CurrentExtraDay = id;
            HeadlineExtraClasses.text = ((DayOfWeek)(id + 1)).ConvertToString(false);
            FillExtraClasses();
        }

        void HideAll()
        {
            Main.SetActive(false);
            Timetable.SetActive(false);
            LessonSchedule.SetActive(false);
            ExtraClasses.SetActive(false);
            DatePanel.SetActive(false);
            WeeklySchedule.SetActive(false);
            DatePanelImage.color = Gray;
        }

        void GenerateClassesButtons()
        {
            int id = 0;
            int lenMas = Stock.Instance.LessonScheduleMatrix[2].Length;

            for (int i = 0; i < lenMas; i++)
            {
                if (Stock.Instance.LessonScheduleMatrix[2][i] == "")
                    continue;

                string ClassName = Stock.Instance.LessonScheduleMatrix[2][i];
                int len = ClassName.Length;
                int number;

                if (int.TryParse(ClassName.Substring(0, len - 1), out number))
                    if (number >= 5 && number <= 11)
                    {
                        GameObject go = Instantiate(ClassesButton, ClassButtons[number - 5].transform);
                        go.name = "" + id;
                        go.GetComponentInChildren<Text>().text = ClassName;
                        id++;
                    }
            }
        }

        public void OnClickBlindMode()
        {
            BlindMode = !BlindMode;
            StartAnimBlind = Time.time;
        }
    }
}