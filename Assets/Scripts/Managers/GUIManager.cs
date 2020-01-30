using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField, Parameter] private AnimatorParameter _fadeParameter;
    [SerializeField] private FadeScreen _homeFadeScreen, _gameFadeScreen;
    
    public AnimatorParameter FadeParameter => _fadeParameter;
    
    public void Init()
    {
        _homeFadeScreen.Hide(false);
        _gameFadeScreen.Hide();
    }

    public void OnClickPlayButton()
    {
        _homeFadeScreen.Fade(true);
        InstanceManager.TurnManager.StartGame();
    }

    public void OnClickNewGameButton()
    {
    }

    public void OnClickQuitButton() => InstanceManager.GameManager.QuitGame();
    
    public void OnClickConfirmButton() => InstanceManager.TurnManager.ConfirmCard();
    
    public void OnClickCancelButton() => InstanceManager.TurnManager.CancelCard();
}