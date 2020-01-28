using MightyAttributes;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField, DrawSerializable] private AnimatorParameter _fadeParameter;
    [SerializeField] private FadeScreen _homeFadeScreen, _gameFadeScreen;

    private static GUIManager m_instance;

#if UNITY_EDITOR
    public static GUIManager Instance => m_instance ? m_instance : m_instance = EditModeUtility.FindFirstObject<GUIManager>();
#else
    public static GUIManager Instance => m_instance;
#endif

    public static AnimatorParameter FadeParameter => Instance._fadeParameter;

    public void Init()
    {
        m_instance = this;
        
        _homeFadeScreen.Hide(false);
        _gameFadeScreen.Hide();
    }
    
    public void OnClickPlayButton()
    {
        _homeFadeScreen.Fade(true);
    }
    
    public void OnClickRestartButton()
    {
        
    }

    public void OnClickQuitButton() => GameManager.Instance.QuitGame();
}
