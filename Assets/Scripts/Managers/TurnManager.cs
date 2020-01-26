using UnityEngine;

public enum TurnState : byte
{
    NotStarted,
    Sommeil,
    Objet,
    Resultat
}

public class TurnManager : MonoBehaviour
{
    #region Serialized

    [SerializeField, Manager] private ScannerManager _scannerManager;
    [SerializeField, Manager] private VideoManager _videoManager;

    #endregion /Serialized

    private TurnState m_turnState;
    private SommeilModel m_currentSommeil;
    private ObjetModel m_currentObjet;
    private ResultatModel m_currentResultat;

    public void Init()
    {
        
    }

    public void StartTurn() => SelectState(TurnState.Sommeil);

    public void StopTurn() => SelectState(TurnState.NotStarted);

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
            case TurnState.Objet:
                _scannerManager.Scan();
                break;
            case TurnState.Resultat:
                SelectResultat(EntitiesDatabase.GetResultat(m_currentSommeil, m_currentObjet));
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
            case TurnState.Objet:
                if (card is ObjetModel objet)
                {
                    SelectObjet(objet);
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

    private void SelectObjet(ObjetModel objet)
    {
        m_currentObjet = objet;
        // PLAY ACTION ANIMATION
    }

    private void SelectResultat(ResultatModel resultat)
    {
        if (resultat == null)
        {
            // DEAL WITH ECHEC
            return;
        }

        m_currentResultat = resultat;
        _videoManager.Play(resultat.videoClip);
        // PLAY RESULTAT ANIMATION

        resultat.UnlockNoteCarnet();
    }
}