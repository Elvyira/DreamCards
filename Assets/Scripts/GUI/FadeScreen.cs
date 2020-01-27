using MightyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AnimatorParameterBehaviour))]
public class FadeScreen : MonoBehaviour
{
    [SerializeField, Hide, GetComponent] private AnimatorParameterBehaviour _animatorParameterBehaviour;

    public void Fade(AnimatorParameter fadeParameter, bool fadeIn)
    {
        gameObject.SetActive(true);
        _animatorParameterBehaviour.SetBool(fadeParameter, fadeIn);
    }

    public void Hide() => gameObject.SetActive(false);
}
