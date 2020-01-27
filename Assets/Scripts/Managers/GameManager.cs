#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Serialized

    [SerializeField, Manager] private GUIManager _guiManager;
    [SerializeField, Manager] private TurnManager _turnManager;
    [SerializeField, Manager] private ScannerManager _scannerManager;
    [SerializeField, Manager] private VideoManager _videoManager;

    #endregion /Serialized

    #region Instance

    private static GameManager m_instance;

#if UNITY_EDITOR
    public static GameManager Instance => m_instance ? m_instance : m_instance = EditModeUtility.FindFirstObject<GameManager>();
#else
    public static GameManager Instance => m_instance;
#endif

    #endregion /Instance

    private void Awake()
    {
        m_instance = this;
        SavedDataServices.LoadEverything();

        _guiManager.Init();
        _turnManager.Init();
        _scannerManager.Init();
        _videoManager.Init();
    }

    private void Update()
    {
        _scannerManager.UpdateManager();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}