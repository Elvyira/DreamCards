using MightyAttributes;
using UnityEngine;

[RequireComponent(typeof(AnimatorParameterBehaviour))]
public class FadeScreen : MonoBehaviour
{
    private const string FADE_START = "FadeStartClip";
    
    [SerializeField, Hide, GetComponent] private AnimatorParameterBehaviour _parameterBehaviour;
    [SerializeField, Hide, GetComponent] private AnimationClipsSwapper _clipsSwapper;
    [SerializeField] private GameObject _menu;

    [SerializeField] private AnimationClip _disableFadeStartClip, _enableFadeStartClip;
    public bool disableOnStart;
    
    public bool hasNextFadeScreen;
    [SerializeField, ShowIf("hasNextFadeScreen")] private FadeScreen _nextFadeScreen;

    public void Fade(bool fadeIn)
    {
        _clipsSwapper.SwapClip(FADE_START, disableOnStart ? _disableFadeStartClip : _enableFadeStartClip);
        _clipsSwapper.ApplyChanges();
        
        _menu.SetActive(true);
        gameObject.SetActive(true);
        
        _parameterBehaviour.SetBool(fadeIn);
    }

    public void Hide(bool hideMenu = true)
    {
        gameObject.SetActive(false);
        _menu.SetActive(!hideMenu);
    }

    public void FadeOutNextScreen() => FadeNextScreen(false);

    public void FadeNextScreen(bool fadeIn)
    {
        Hide();
        if (!hasNextFadeScreen) return;
        
        _nextFadeScreen.disableOnStart = false;
        _nextFadeScreen.Fade(fadeIn);
    }
}
