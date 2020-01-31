using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private FadeScreen _homeFadeScreen, _gameFadeScreen;
    [SerializeField] private TextMeshProUGUI _resultLabel;
    [SerializeField] private Cadran _cadran;
    
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

    public void OnShowResult(string resultLabel)
    {
        _resultLabel.gameObject.SetActive(true);
        _resultLabel.text = resultLabel;
    }

    public void OnChangeTurn(int turn) => _cadran.SetTurn(turn);

    public void HideResult() => _resultLabel.gameObject.SetActive(false);

    public void FadeToHome()
    {
        _gameFadeScreen.disableOnStart = true;
        _gameFadeScreen.Fade(true);
    }
}