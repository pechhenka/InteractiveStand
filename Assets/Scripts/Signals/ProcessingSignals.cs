using System;
using System.Collections.Generic;

namespace Stand
{
    public class ProcessingSignals : IDisposable
    {
        public static ProcessingSignals Default = new ProcessingSignals();
        public readonly Dictionary<int, List<IRecieve>> signals = new Dictionary<int, List<IRecieve>>(new FastDict());


        public void Add(IRecieve recieve, Type type)
        {
            if (signals.TryGetValue(type.GetHashCode(), out List<IRecieve> cachedSignals))
            {
                cachedSignals.Add(recieve);
                return;
            }

            signals.Add(type.GetHashCode(), new List<IRecieve> { recieve });
        }

        public void Remove(IRecieve recieve, Type type)
        {
            if (signals.TryGetValue(type.GetHashCode(), out List<IRecieve> cachedSignals))
            {
                cachedSignals.Remove(recieve);
            }
        }


        public void Add(object obj)
        {
            var all = obj.GetType().GetInterfaces();
            var reciever = obj as IRecieve;
            foreach (var intType in all)
            {
                if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IReceive<>))
                {
                    Add(reciever, intType.GetGenericArguments()[0]);
                }
            }
        }

        public void Remove(object obj)
        {
            var all = obj.GetType().GetInterfaces();
            var reciever = obj as IRecieve;
            foreach (var intType in all)
            {
                if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IReceive<>))
                {
                    Remove(reciever, intType.GetGenericArguments()[0]);
                }
            }
        }

        public void Send<T>(T val)
        {
            if (!signals.TryGetValue(typeof(T).GetHashCode(), out List<IRecieve>  cachedSignals)) return;
            for (int i = cachedSignals.Count - 1; i >= 0; --i) // ������ ���������� ����� ������ ��������� �������� �� �����
                (cachedSignals[i] as IReceive<T>)?.HandleSignal(val);
        }

        public void Dispose()
        {
            signals.Clear();
        }

        public class FastDict : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }
    }
    public interface IReceive<T> : IRecieve
    {
        void HandleSignal(T arg);
    }
    public interface IRecieve
    {
    }
}