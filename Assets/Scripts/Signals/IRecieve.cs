namespace Stand
{
    public interface IReceive<T> : IRecieve
    {
        void HandleSignal(T arg);
    }

    public interface IReceiveGlobal<T> : IReceive<T>
    {
    }

    public interface IRecieve
    {
    }

    public interface IReciever
    {
        void StartRecieve();
    }
}