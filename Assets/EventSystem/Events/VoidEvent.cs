using UnityEngine;

namespace CustomEventSystem
{
    [CreateAssetMenu(fileName = "New Void Event", menuName = "Game Events/Void events")]
    public class VoidEvent : BaseEvent<Void>
    {
        public void Raise() => Raise(new Void());
    }
}
