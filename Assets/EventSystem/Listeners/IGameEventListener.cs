namespace CustomEventSystem
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}

