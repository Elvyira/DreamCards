using MightyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameLoopController : MonoBehaviour
{
    private const string START_SOMMEIL = "StartSommeilClip";
    private const string IDLE_SOMMEIL = "IdleSommeilClip";
    private const string USE_OBJET = "UseObjetClip";
    private const string RESULT = "ResultClip";

    [SerializeField, ComponentReadOnly] private AnimatorParameterBehaviour _animatorBehaviour;
    [SerializeField, ComponentReadOnly] private AnimationClipsSwapper _clipsSwapper;
    [SerializeField, AssetOnly] private Sprite _sommeilIcon, _objetIcon;
    [SerializeField] private Image _cardImage, _scanCardImage;

    [SerializeField, Parameter] private AnimatorParameter _advanceParameter;
    [SerializeField, Parameter] private AnimatorParameter _exitParameter;
    [SerializeField, Parameter] private AnimatorParameter _sommeilParameter;

    public void Init() => _clipsSwapper.Init();

    public void OnSelectCard(CardModel card)
    {
        _animatorBehaviour.SetTrigger(_advanceParameter);
        _cardImage.sprite = card.CardSprite;
        switch (card)
        {
            case SommeilModel sommeilModel:
                SetSommeilClips(sommeilModel);
                break;
            case ObjetModel objetModel:
                SetObjetClip(objetModel);
                break;
        }
    }

    public void OnConfirmCard(bool sommeil)
    {
        _animatorBehaviour.SetBool(_sommeilParameter, sommeil);
        _animatorBehaviour.SetTrigger(_advanceParameter);
        _scanCardImage.sprite = sommeil ? _objetIcon : _sommeilIcon;
    }

    public void OnCancelCard() => _animatorBehaviour.SetTrigger(_exitParameter);

    public void OnSelectResultat(ResultatModel resultat) => SetResultClip(resultat);

    public void EndNight() => _animatorBehaviour.SetTrigger(_exitParameter);

    private void SetSommeilClips(SommeilModel sommeil)
    {
        _clipsSwapper.SwapClip(START_SOMMEIL, sommeil.startClip);
        _clipsSwapper.SwapClip(IDLE_SOMMEIL, sommeil.idleClip);
        _clipsSwapper.ApplyChanges();
    }

    private void SetObjetClip(ObjetModel objet)
    {
        _clipsSwapper.SwapClip(USE_OBJET, objet.objetClip);
        _clipsSwapper.ApplyChanges();
    }

    private void SetResultClip(ResultatModel resultat)
    {
        _clipsSwapper.SwapClip(RESULT, resultat ? resultat.resultClip : null);
        _clipsSwapper.ApplyChanges();
    }
}