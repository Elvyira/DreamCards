using UnityEngine;

public abstract class BaseEventsBehaviour : MonoBehaviour
{
    private bool m_ignoreOnce;

    public void IgnoreOnce() => m_ignoreOnce = true;
    public void StopIgnore() => m_ignoreOnce = false;

    protected bool ShouldIgnore()
    {
        if (!m_ignoreOnce) return false;
        
        m_ignoreOnce = false;
        return true;
    }
}
