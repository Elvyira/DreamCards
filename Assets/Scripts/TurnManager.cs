using UnityEngine;

public enum TurnState : byte
{
    NotStarted,
    Sommeil,
    Action,
    Resultat
}

public class TurnManager : MonoBehaviour
{
    [SerializeField] private ScannerManager _scannerManager;
    [SerializeField] private VideoManager _videoManager;

    private TurnState m_turnState;
    private SommeilModel m_currentSommeil;
    private ActionModel m_currentAction;
    private ResultatModel m_currentResultat;

    public void StartTurn()
    {
        SelectState(TurnState.Sommeil);
    }

    public void StopTurn()
    {
        SelectState(TurnState.NotStarted);
    }

    public void GoToNextState()
    {
        var newState = m_turnState + 1;

        if (newState <= TurnState.Resultat)
            SelectState(newState);
        else
            StopTurn();
    }

    public void SelectState(TurnState state)
    {
        m_turnState = state;
        switch (state)
        {
            case TurnState.NotStarted:
                _scannerManager.Stop();
                break;
            case TurnState.Sommeil:
                _scannerManager.Scan();
                break;
            case TurnState.Action:
                _scannerManager.Scan();
                break;
            case TurnState.Resultat:
                SelectResultat(EntitiesDatabase.GetResultat(m_currentSommeil, m_currentAction));
                break;
        }
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
        _videoManager.Play(sommeil.startVideoClip, sommeil.idleVideoClip);
    }

    private void SelectAction(ActionModel action)
    {
        m_currentAction = action;
        // PLAY ACTION ANIMATION
    }

    private void SelectResultat(ResultatModel resultat)
    {
        m_currentResultat = resultat;
        _videoManager.Play(resultat.videoClip);
       // PLAY RESULTAT ANIMATION
       
       if (resultat.typeResultat != TypeResultat.Echec && resultat.noteCarnet != null) 
           EntitiesDatabase.UnlockNotebookEntry(resultat.noteCarnet);
    }
}