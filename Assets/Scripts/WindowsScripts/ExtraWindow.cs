using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class ExtraWindow : WindowBase
    {
        [Header("Prefabs")]
        public GameObject InformationBlockPrefab;
        public GameObject Dot;

        [Header("Others")]
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

        private DayOfWeek CurrentExtraDay = 0;
        private int CurrentExtraBlock = 0;
        private int LastExtraBlock = 0;
        private int LengthExtraBlocks = 0;
        private bool ActivateNextRightBlock = false;
        private bool EndAnimation = false;

        private void FixedUpdate()
        {
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
        }

        public void BackInformationBlock()
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

        public void NextInformationBlock()
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
            Dots.transform.GetChild(LastExtraBlock).GetComponent<Image>().color = _UIController.LowGray;
            Dots.transform.GetChild(CurrentExtraBlock).GetComponent<Image>().color = Color.white;
            StartAnimation = Time.time;

            RectTransform CurrentEIB = InformationsBlocks.transform.GetChild(CurrentExtraBlock).GetComponent<RectTransform>();
            if (ActivateNextRightBlock)
                CurrentEIB.anchoredPosition = EndPos;
            else
                CurrentEIB.anchoredPosition = EndPos.NegativeX();
            CurrentEIB.localScale = EndScale;
        }

        public override void PrimaryFill()
        {
            HeadlineExtraClasses.text = DateTime.Now.DayOfWeek.ConvertToString(false);
            CurrentExtraDay = DateTime.Now.DayOfWeek;
            Fill();
        }

        public override void Refill() => PrimaryFill();
        public override void Fill()
        {
            Loger.Log($"Окно доп.секций&IdDay:{CurrentExtraDay}", "открыли");

            foreach (Transform child in InformationsBlocks.transform)
                Destroy(child.gameObject);
            foreach (Transform child in Dots.transform)
                Destroy(child.gameObject);

            Extra extra = ExtraParser.Instance.GetExtra(CurrentExtraDay);
            CurrentExtraBlock = 0;
            LengthExtraBlocks = extra.Amount;

            GameObject go = null;
            if (extra.Amount == 0)
            {
                go = Instantiate(InformationBlockPrefab, InformationsBlocks.transform);
                go.transform.Find("CourseName").GetComponent<Text>().text = "нет данных";
                go.transform.Find("Classes").GetComponent<Text>().text = "";
                go.transform.Find("Time").GetComponent<Text>().text = "";
                go.transform.Find("Сlassroom").GetComponent<Text>().text = "";
                return;
            }
            for (int i = 0;i<extra.Amount;i++)
            {
                go = Instantiate(InformationBlockPrefab, InformationsBlocks.transform);
                go.transform.Find("CourseName").GetComponent<Text>().text = extra.CourseName[i];
                go.transform.Find("Classes").GetComponent<Text>().text = "Классы: " + extra.Classes[i];
                go.transform.Find("Time").GetComponent<Text>().text = "Время: " + extra.Time[i];
                go.transform.Find("Сlassroom").GetComponent<Text>().text = "Кабинет: " + extra.Сlassroom[i];

                if (i == 0)
                {
                    Instantiate(Dot, Dots.transform).GetComponent<Image>().color = Color.white;
                }
                else
                {
                    go.GetComponent<CanvasGroup>().alpha = 0f;
                    Instantiate(Dot, Dots.transform);
                }                    
            }
        }
        public override void Fill(int id) => PrimaryFill();
        public override void Fill(GameObject gameObject) => PrimaryFill();
        public override void ChooseClass(string Class) => PrimaryFill();
        public void ChooseDay(int d) => ChooseDay(d.ToDayOfWeek());
        public override void ChooseDay(DayOfWeek d)
        {
            if (d != CurrentExtraDay)
            {
                CurrentExtraDay = d;
                HeadlineExtraClasses.text = d.ConvertToString(false);
                Fill();
            }
            else
            {
                Loger.Log($"Окно доп.секций&IdDay:{CurrentExtraDay}", "нервное касание");
            }
        }

        public override void Merge(bool Status) { }
    }
}