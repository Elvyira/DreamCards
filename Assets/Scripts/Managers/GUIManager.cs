using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private FadeScreen _homeFadeScreen, _gameFadeScreen;
    
    public void Init()
    {
        _homeFadeScreen.Hide(false);
        _gameFadeScreen.Hide();
    }

    public void OnClickNewGameButton()
    {
    }

    public void OnClickQuitButton() => InstanceManager.GameManager.QuitGame();

    public void OnClickCancelButton() => InstanceManager.TurnManager.CancelCard();
}