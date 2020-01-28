using MightyAttributes;
using UnityEngine;

[RequireComponent(typeof(AnimatorParameterBehaviour))]
public class FadeScreen : MonoBehaviour
{
    [SerializeField, Hide, GetComponent] private AnimatorParameterBehaviour _animatorParameterBehaviour;
    [SerializeField] private GameObject _menu;
    public bool hasNextFadeScreen;
    [SerializeField, ShowIf("hasNextFadeScreen")] private FadeScreen _nextFadeScreen;

    public void Init()
    {
        
    }
    
    public void Fade(bool fadeIn)
    {
        _menu.SetActive(true);
        gameObject.SetActive(true);
        _animatorParameterBehaviour.SetBool(GUIManager.FadeParameter, fadeIn);
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
        if (hasNextFadeScreen) _nextFadeScreen.Fade(fadeIn);
    }
}
