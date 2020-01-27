using MightyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsBehaviour : BaseEventsBehaviour
{
    [SerializeField, ButtonArray(ArrayOption.HideLabel)] private UnityEvent[] _events;

    public void TriggerAnimationEvent(int index)
    {
        if (ShouldIgnore()) return;
        if (index < _events.Length)
            _events[index].Invoke();
    }
}