using UnityEngine;
using UnityEngine.Events;

namespace CustomEventSystem
{
    //T = Type // E = Event // UER = Unity event Response
    public abstract class BaseEventListener<T, E, UER> : MonoBehaviour,
        IGameEventListener<T> where E : BaseEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField] private E gameEvent;
        public E GameEvent { get { return gameEvent; } set { gameEvent = value; } }

        [SerializeField] private UER unityEventResponse;


        //Only listen if exists in scene
        //register
        private void OnEnable()
        {
            if (gameEvent == null)
            {
                return;
            }
            GameEvent.RegisterListener(this);
        }

        //unregister
        private void OnDisable()
        {
            if (gameEvent == null)
            {
                return;
            }
            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if (unityEventResponse != null)
            {
                unityEventResponse.Invoke(item);
            }
        }
    }

}
