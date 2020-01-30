using System.Collections.Generic;
using MightyAttributes;
using UnityEngine;

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) {}

    public AnimationClip this[string name]
    {
        get { return Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            var index = FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

public class AnimationClipsSwapper : MonoBehaviour
{
    public Animator animator;
    
#if UNITY_EDITOR
    [Button]
    private void AnimatorFromSelf() => animator = GetComponent<Animator>();
#endif

    private AnimatorOverrideController m_overrideController;
    private AnimationClipOverrides m_clipOverrides;

    private bool m_init;

    public void Init()
    {
        ForceInit();
        m_init = true;
    }

    public void ForceInit()
    {
        m_overrideController = (AnimatorOverrideController) animator.runtimeAnimatorController;
        m_clipOverrides = new AnimationClipOverrides(m_overrideController.overridesCount);
        m_overrideController.GetOverrides(m_clipOverrides);
    }

    public void SwapClip(string clipName, AnimationClip clip)
    {
        if (!m_init) Init();
        m_clipOverrides[clipName] = clip;
    }

    public void ApplyChanges()
    {
        if (!m_init) Init();
        m_overrideController.ApplyOverrides(m_clipOverrides);
    }
}
