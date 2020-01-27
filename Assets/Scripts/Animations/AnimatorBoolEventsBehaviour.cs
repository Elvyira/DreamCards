using MightyAttributes;
using UnityEngine;

public class AnimatorBoolEventsBehaviour : BaseEventsBehaviour
{
    [SerializeField] private bool _boolValue;

    [SerializeField, ButtonArray(ArrayOption.HideLabel)] private BoolEvent[] _events;

    public void SetBool(bool value) => _boolValue = value;

    public void TriggerAnimationEvent(int index)
    {
        if (ShouldIgnore()) return;
        if (index < _events.Length)
            _events[index].Invoke(_boolValue);
    }

    public void TriggerAnimationEvent(int index, bool value)
    {
        SetBool(value);
        TriggerAnimationEvent(index);
    }
}