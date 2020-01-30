using MightyAttributes;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    [SerializeField, FindReadOnly] private InstanceManager _instanceManager;

    private bool m_init;
    
    private void Awake() => Init();

    public void Init()
    {
        if (m_init) return;
        ForceInit();
        m_init = true;
    }
    
    [Button]
    public void ForceInit()
    {
        _instanceManager.Init();
        
        InstanceManager.GameManager.Init();
        InstanceManager.GUIManager.Init();
        InstanceManager.TurnManager.Init();
        
        InstanceManager.CustomScannerSettings.Init();
        InstanceManager.ScannerManager.Init();
    }
}