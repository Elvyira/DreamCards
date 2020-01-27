using System;
using MightyAttributes;
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

    public bool hasStateChangeEvent;

    [SerializeField, ShowIf("hasStateChangeEvent")]
    private TurnStateEvent _onStateChange;

    public bool hasResultEvent;

    [SerializeField, ShowIf("hasResultEvent")]
    private StringEvent _onResult;

    #endregion /Serialized

    private TurnState m_turnState;
    private SommeilModel m_currentSommeil;
    private ObjetModel m_currentObjet;
    private ResultatModel m_currentResultat;

    private Action m_sommeilOverAction;
    private Action m_resultatOverAction;

    public void Init()
    {
        m_sommeilOverAction = () => SelectState(TurnState.Objet);
        m_resultatOverAction = () => SelectState(TurnState.NotStarted);
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
        if (hasStateChangeEvent) _onStateChange.Invoke(state);
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
                    _scannerManager.Stop();

                    // TODO: REMOVE WHEN VIDEO WILL BE ADDED
                    //************************************
                    SelectState(TurnState.Objet);
                    //************************************
                }

                break;
            case TurnState.Objet:
                if (card is ObjetModel objet)
                {
                    SelectObjet(objet);
                    _scannerManager.Stop();

                    // TODO: REMOVE WHEN ANIMATION WILL BE ADDED
                    //************************************
                    SelectState(TurnState.Resultat);
                    //************************************
                }

                break;
        }
    }

    private void SelectSommeil(SommeilModel sommeil)
    {
        Debug.Log(sommeil.nom);
        if (hasResultEvent)
            _onResult.Invoke(sommeil.nom);

        m_currentSommeil = sommeil;
        _videoManager.Play(sommeil.startVideoClip, sommeil.idleVideoClip, m_sommeilOverAction);
    }

    private void SelectObjet(ObjetModel objet)
    {
        Debug.Log(objet.nom);
        if (hasResultEvent)
            _onResult.Invoke(objet.nom);

        m_currentObjet = objet;
        //TODO: PLAY ACTION ANIMATION
    }

    private void SelectResultat(ResultatModel resultat)
    {
        if (resultat == null)
        {
            //TODO: DEAL WITH ECHEC
            if (hasResultEvent)
                _onResult.Invoke("Echec");
            return;
        }

        if (hasResultEvent)
            _onResult.Invoke(resultat.typeResultat.ToString());

        m_currentResultat = resultat;
        _videoManager.Play(resultat.videoClip, m_resultatOverAction);
        //TODO: PLAY RESULTAT ANIMATION

        resultat.UnlockNoteCarnet();
    }
}