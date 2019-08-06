using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class LessonsWindow : WindowBase
    {
        [Header("Prefabs")]
        public GameObject ClassesButton;

        [Header("Others")]
        public GameObject[] ClassButtons;

        public override void PrimaryFill()
        {
            int lenMas = Data.Instance.LessonsMatrix.GetRow(2).LastCellNum;

            for (int i = 0; i < lenMas; i++)
            {
                if (Data.Instance.LessonsMatrix.GetCell(2, i) == "")
                    continue;

                string ClassName = Data.Instance.LessonsMatrix.GetCell(2, i);
                int len = ClassName.Length;
                int number;

                if (int.TryParse(ClassName.Substring(0, len - 1), out number))
                    if (number >= 5 && number <= 11)
                    {
                        GameObject go = Instantiate(ClassesButton, ClassButtons[number - 5].transform);
                        go.name = "" + i;
                        go.GetComponentInChildren<Text>().text = ClassName;
                    }
            }
        }

        public override void Refill() => PrimaryFill();
        public override void Fill() => PrimaryFill();
        public override void Fill(int id) => PrimaryFill();
        public override void Fill(GameObject gameObject) => PrimaryFill();
        public override void ChooseClass(string Class) => PrimaryFill();
        public override void ChooseDay(int id) => PrimaryFill();

        public override void Merge(bool Status) { }
    }
}