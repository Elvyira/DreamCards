using MightyAttributes;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField, DrawSerializable] private AnimatorParameter _fadeParameter;
    [SerializeField] private FadeScreen _homeFadeScreen, _gameFadeScreen;
    [SerializeField] private GameObject _homeMenu, _gameMenu;

    public void Init()
    {
        _homeMenu.SetActive(true);
        _gameMenu.SetActive(false);
        
        _homeFadeScreen.Hide();
        _gameFadeScreen.Hide();
    }
    
    public void OnClickPlayButton()
    {
        _homeFadeScreen.Fade(_fadeParameter, true);
    }
    
    public void OnClickRestartButton()
    {
        
    }

    public void OnClickQuitButton() => GameManager.Instance.QuitGame();

    public void OnHomeHidden()
    {
        _homeMenu.SetActive(false);
        _homeFadeScreen.Hide();

        _gameMenu.SetActive(true);
        _gameFadeScreen.Fade(_fadeParameter, false);
    }   
    
    public void OnGameHidden()
    {
        _gameMenu.SetActive(false);
        _gameFadeScreen.Hide();
        
        _homeMenu.SetActive(true);
        _homeFadeScreen.Fade(_fadeParameter, false);
    }
}
