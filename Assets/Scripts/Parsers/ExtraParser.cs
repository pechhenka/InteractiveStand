using System;

namespace Stand
{
    public class ExtraParser : Parser<ExtraParser>, IReciever, IReceive<SignalExtraMatrixChanged>
    {
        void IReciever.StartRecieve()
        {
            ProcessingSignals.Default.Add(this);
        }

        public Extra GetExtra(DayOfWeek d)
        {
            if (Data.Instance.ExtraMatrix == null || d == DayOfWeek.Sunday) return new Extra();

            int Len = Data.Instance.ExtraMatrix.LastRowNum;
            int d_Norm = d.Normalising();
            int j = 0;
            Extra res = new Extra(null,null,null,null,0);

            for (int i = 1 + Data.Instance.CurrentManifest.ExtraMatrixOffsetY; i <= Len; i++)
            {
                j = Data.Instance.CurrentManifest.ExtraMatrixOffsetX + d_Norm * 4;
                if (Data.Instance.ExtraMatrix.GetCell(i, j) == "") continue;

                res.CourseName.Add(Data.Instance.ExtraMatrix.GetCell(i, j));
                res.Classes.Add(Data.Instance.ExtraMatrix.GetCell(i, j + 1));
                res.Time.Add(Data.Instance.ExtraMatrix.GetCell(i, j + 2));
                res.Сlassroom.Add(Data.Instance.ExtraMatrix.GetCell(i, j + 3));
                res.Amount++;
            }

            return res;
        }

        void IReceive<SignalExtraMatrixChanged>.HandleSignal(SignalExtraMatrixChanged arg)
        {
            throw new System.NotImplementedException();
        }
    }
}
