using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class CallsWindow : WindowBase, IReceive<SignalCallsMatrixChanged>
    {
        public Text Monday;
        public Text Tuesday;
        public Text Saturday;

        public override void PrimaryFill()
        {
            string[] res = new string[3];
            res[0] = Data.Instance.CallsMatrix.GetCell(0, 0) + Environment.NewLine;
            res[1] = Data.Instance.CallsMatrix.GetCell(0, 1) + Environment.NewLine;
            res[2] = Data.Instance.CallsMatrix.GetCell(0, 2) + Environment.NewLine;

            bool Switch = false;
            foreach (TimeSpan item in CallsParser.Instance.GetColumnWithoutChanges(0))
            {
                res[0] += item.ToTimeString() + (Switch ? Environment.NewLine : " - ");
                Switch = !Switch;
            }

            Switch = false;
            foreach (TimeSpan item in CallsParser.Instance.GetColumnWithoutChanges(1))
            {
                res[1] += item.ToTimeString() + (Switch ? Environment.NewLine : " - ");
                Switch = !Switch;
            }

            Switch = false;
            foreach (TimeSpan item in CallsParser.Instance.GetColumnWithoutChanges(2))
            {
                res[2] += item.ToTimeString() + (Switch ? Environment.NewLine : " - ");
                Switch = !Switch;
            }

            Monday.text = res[0];
            Tuesday.text = res[1];
            Saturday.text = res[2];
        }

        public override void Refill() => PrimaryFill();
        public override void Fill() => PrimaryFill();
        public override void Fill(int id) => PrimaryFill();
        public override void Fill(GameObject gameObject) => PrimaryFill();
        public override void ChooseClass(string Class) => PrimaryFill();
        public override void ChooseDay(int id) => PrimaryFill();

        public override void Merge(bool Status) { }

        void IReceive<SignalCallsMatrixChanged>.HandleSignal(SignalCallsMatrixChanged arg) => PrimaryFill();
    }
}