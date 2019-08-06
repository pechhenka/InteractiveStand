using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stand
{
    public class LessonsParser : Parser<LessonsParser>, IReciever, IReceive<SignalLessonsMatrixChanged>, IReceive<SignalChangeLessonsMatrixChanged>
    {
        void IReceive<SignalLessonsMatrixChanged>.HandleSignal(SignalLessonsMatrixChanged arg)
        {
            throw new System.NotImplementedException();
        }

        void IReceive<SignalChangeLessonsMatrixChanged>.HandleSignal(SignalChangeLessonsMatrixChanged arg)
        {
            throw new System.NotImplementedException();
        }

        void IReciever.StartRecieve()
        {
            ProcessingSignals.Default.Add(this);
        }
    }
}