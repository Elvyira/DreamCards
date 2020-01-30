using MightyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopController : MonoBehaviour
{
    private const string START_SOMMEIL = "StartSommeilClip";
    private const string IDLE_SOMMEIL = "IdleSommeilClip";
    private const string USE_OBJET = "UseObjetClip";
    private const string RESULT = "ResultClip";

    [SerializeField, ComponentReadOnly] private AnimatorParameterBehaviour _parameterBehaviour;
    [SerializeField, ComponentReadOnly] private AnimationClipsSwapper _clipsSwapper;
    
    [SerializeField, AssetOnly] private Sprite _sommeilIcon, _objetIcon;
    
    [SerializeField] private Image _cardImage, _scanCardImage;
    [SerializeField] private Button _confirmButton, _cancelButton;

    [SerializeField] private AnimatorParameterBehaviour _diaryParameterBehaviour, _scanCardParameterBehaviour;

    [SerializeField, Parameter] private AnimatorParameter _advanceParameter;
    [SerializeField, Parameter] private AnimatorParameter _exitParameter;
    [SerializeField, Parameter] private AnimatorParameter _sommeilParameter;
    [SerializeField, Parameter] private AnimatorParameter _nightOverParameter;

    public void ShowScanIcon(bool show) => _scanCardParameterBehaviour.gameObject.SetActive(show);
    public void ShowDiaryIcon(bool show) => _diaryParameterBehaviour.gameObject.SetActive(show);

    public void OnSelectCard(CardModel card)
    {
        _parameterBehaviour.SetTrigger(_advanceParameter);
        
        _confirmButton.enabled = _cancelButton.enabled = true;
        _cardImage.sprite = card.CardSprite;
        
        _scanCardParameterBehaviour.SetTrigger(_exitParameter);
        
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
        _parameterBehaviour.SetBool(_sommeilParameter, sommeil);
        _parameterBehaviour.SetTrigger(_advanceParameter);
        _scanCardImage.sprite = sommeil ? _objetIcon : _sommeilIcon;
    }

    public void OnCancelCard()
    {
        ShowScanIcon(true);
        _parameterBehaviour.SetTrigger(_exitParameter);
    }

    public void OnSelectResultat(ResultatModel resultat) => SetResultClip(resultat);

    public void HasNextTurn(bool hasNext) => _parameterBehaviour.SetBool(_nightOverParameter, !hasNext);

    public void EndNight()
    {
        _diaryParameterBehaviour.SetTrigger(_exitParameter);
        _parameterBehaviour.SetTrigger(_exitParameter);
    }

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
        _parameterBehaviour.animator.GetBehaviour<SFXBehaviour>().source = resultat ? resultat.typeResultat.GetClip() : SFXSource.Echec;
        
        _clipsSwapper.SwapClip(RESULT, resultat ? resultat.resultClip : null);
        _clipsSwapper.ApplyChanges();
    }
}