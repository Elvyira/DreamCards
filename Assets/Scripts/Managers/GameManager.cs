#if UNITY_EDITOR
using UnityEditor;
#endif
using MightyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Serialized

    [SerializeField, FindReadOnly] private EntitiesDatabase _entitiesDatabase;
    
    [SerializeField, FindReadOnly] private GUIManager _guiManager;
    [SerializeField, FindReadOnly] private TurnManager _turnManager;
    [SerializeField, FindReadOnly] private ScannerManager _scannerManager;
    [SerializeField, FindReadOnly] private VideoManager _videoManager;

    #endregion /Serialized

    #region Instance

    private static GameManager m_instance;

#if UNITY_EDITOR
    public static GameManager Instance => m_instance ? m_instance : m_instance = EditModeUtility.FindFirstObject<GameManager>();
#else
    public static GameManager Instance => m_instance;
#endif

    #endregion /Instance

    private void Awake() => Init();

    [Button]
    public void Init()
    {
        m_instance = this;
        SavedDataServices.LoadEverything();
        
        _entitiesDatabase.Init();

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