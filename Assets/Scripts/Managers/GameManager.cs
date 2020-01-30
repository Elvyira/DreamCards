#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ScannerManager m_scannerManager;
    
    public void Init()
    {
        SavedDataServices.LoadEverything();
        m_scannerManager = InstanceManager.ScannerManager;
    }

    private void Update()
    {
        m_scannerManager.UpdateManager();
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