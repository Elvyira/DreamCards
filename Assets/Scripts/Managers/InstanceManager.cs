using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    // @formatter:off
    [Header("Managers")]
    [SerializeField, FindReadOnly] private GameManager _gameManager;
    [SerializeField, FindReadOnly] private GUIManager _guiManager;
    [SerializeField, FindReadOnly] private TurnManager _turnManager;
    [SerializeField, FindReadOnly] private ScannerManager _scannerManager;
    [SerializeField, FindReadOnly] private EntitiesManager _entitiesManager;

    [Header("Others")] 
    [SerializeField, FindReadOnly] private GameLoopController _gameLoopController;
    // @formatter:on

    private static InstanceManager m_instance;

#if UNITY_EDITOR
    public static InstanceManager Instance => m_instance ? m_instance : m_instance = EditModeUtility.FindFirstObject<InstanceManager>();
#else
    public static InstanceManager Instance => m_instance;
#endif

    public void Init() => m_instance = this;

    public static GameManager GameManager => Instance._gameManager;
    public static GUIManager GUIManager => Instance._guiManager;
    public static TurnManager TurnManager => Instance._turnManager;
    public static ScannerManager ScannerManager => Instance._scannerManager;
    public static EntitiesManager EntitiesManager => Instance._entitiesManager;

    public static GameLoopController GameLoopController => Instance._gameLoopController;
}