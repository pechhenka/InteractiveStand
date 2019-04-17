using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class CallsWindow : IWindow
    {
        public Text Monday;
        public Text Tuesday;
        public Text Saturday;

        public override void PrimaryFill()
        {
            Monday.text = "";
            Tuesday.text = "";
            Saturday.text = "";

            foreach (string[] s in Data.Instance.TimetableMatrix)
            {
                Monday.text += s[0] + '\n';
                Tuesday.text += s[1] + '\n';
                Saturday.text += s[2] + '\n';
            }
        }

        public override void Refill() => PrimaryFill();
        public override void Fill() => PrimaryFill();
        public override void Fill(int id) => PrimaryFill();
        public override void Fill(GameObject gameObject) => PrimaryFill();
    }
}