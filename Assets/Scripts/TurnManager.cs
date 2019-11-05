using UnityEngine;
using UnityEngine.Video;

public enum TurnState
{
    Sommeil,
    Action
}

public class TurnManager : MonoBehaviour
{
    [SerializeField] private ScannerManager _scannerManager;
    [SerializeField] private VideoPlayer _videoPlayer;

    private TurnState m_turnState;
    private SommeilModel m_currentSommeil;
    private ActionModel m_currentAction;

    private TypeResultat m_currentTypeResultat;

    public void StartTurn()
    {
        _scannerManager.Scan();
        m_turnState = TurnState.Sommeil;
    }

    public void SelectCard(CardModel card)
    {
        switch (m_turnState)
        {
            case TurnState.Sommeil:
                if (card is SommeilModel sommeil)
                {
                    SelectSommeil(sommeil);
                }
                else
                {
                    // WRONG CARD TYPE SCANNED
                    _scannerManager.Scan();
                }

                break;
            case TurnState.Action:
                if (card is ActionModel action)
                {
                    SelectAction(action);
                }
                else
                {
                    // WRONG CARD TYPE SCANNED
                    _scannerManager.Scan();
                }

                break;
        }
    }

    private void SelectSommeil(SommeilModel sommeil)
    {
        m_currentSommeil = sommeil;
        _videoPlayer.clip = sommeil.startVideoClip;
        _videoPlayer.Play();
        m_turnState = TurnState.Action;
    }

    private void SelectAction(ActionModel action)
    {
        m_currentAction = action;
        // PLAY ACTION ANIMATION
    }

    public void TryAction()
    {
        if (m_currentSommeil == null || m_currentAction == null) return;

        m_currentTypeResultat = m_currentSommeil.TryAction(m_currentAction);
    }
}