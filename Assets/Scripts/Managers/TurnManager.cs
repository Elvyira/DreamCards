using System;
using MightyAttributes;
using UnityEngine;

public enum TurnState : byte
{
    NotStarted,
    Sommeil,
    Objet
}

public class TurnManager : MonoBehaviour
{
    #region Serialized

    public int numberOfTurns;

    public bool hasStateChangeEvent;

    [SerializeField, ShowIf("hasStateChangeEvent")]
    private TurnStateEvent _onStateChange;

    public bool hasResultEvent;

    [SerializeField, ShowIf("hasResultEvent")]
    private StringEvent _onResult;

    #endregion /Serialized

    private ScannerManager m_scannerManager;
    private GUIManager m_guiManager;
    private GameLoopController m_gameLoopController;

    private TurnState m_turnState;
    private SommeilModel m_currentSommeil;
    private TypeResultat? m_typeResultat;

    private int m_turnCount;

    public TurnState TurnState => m_turnState;

    public void Init()
    {
        m_scannerManager = InstanceManager.ScannerManager;
        m_guiManager = InstanceManager.GUIManager;
        m_gameLoopController = InstanceManager.GameLoopController;
    }

    public void StartGame()
    {
        m_gameLoopController.ShowScanIcon(false);
        m_gameLoopController.ShowDiaryIcon(false);
        SelectState(TurnState.NotStarted);
    }

    public void SelectState(TurnState state)
    {
        m_turnState = state;
        if (hasStateChangeEvent) _onStateChange.Invoke(state);
        switch (state)
        {
            case TurnState.NotStarted:
                m_turnCount = 0;
                m_scannerManager.StopScan();
                break;

            case TurnState.Sommeil:
                m_guiManager.OnChangeTurn(m_turnCount);
                m_turnCount++;
                if (m_turnCount > numberOfTurns)
                {
                    SelectState(TurnState.NotStarted);
                    m_gameLoopController.EndNight();
                    return;
                }

                m_gameLoopController.ShowDiaryIcon(true);
                m_gameLoopController.ShowScanIcon(true);
                m_scannerManager.StartScan();
                break;

            case TurnState.Objet:
                m_gameLoopController.ShowScanIcon(true);
                m_scannerManager.StartScan();
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
                    m_scannerManager.StopScan();
                    SelectSommeil(sommeil);
                }

                break;
            case TurnState.Objet:
                if (card is ObjetModel objet)
                {
                    m_scannerManager.StopScan();
                    SelectObjet(objet);
                }

                break;
        }
    }

    public void ConfirmCard() => m_gameLoopController.OnConfirmCard(m_turnState == TurnState.Sommeil);

    public void CancelCard()
    {
        m_scannerManager.StartScan();
        m_gameLoopController.OnCancelCard();
    }

    public void PlayCardSFX()
    {
        switch (m_turnState)
        {
            case TurnState.Sommeil:
                InstanceManager.AudioManager.PlayClip(m_currentSommeil.audioClip, true);
                break;
            case TurnState.Objet:
                InstanceManager.AudioManager.PlayClip(m_currentSommeil.audioClip, false);
                break;
        }
    }

    public void ShowResult(bool show)
    {
        if (show) m_guiManager.OnShowResult(m_typeResultat == null ? "Echec" : ((TypeResultat) m_typeResultat).PrettyName());
        else m_guiManager.HideResult();
    }

    private void SelectSommeil(SommeilModel sommeil)
    {
        if (hasResultEvent)
            _onResult.Invoke(sommeil.nom);

        m_currentSommeil = sommeil;

        m_gameLoopController.OnSelectCard(sommeil);
    }

    private void SelectObjet(ObjetModel objet)
    {
        if (hasResultEvent)
            _onResult.Invoke(objet.nom);

        m_typeResultat = SelectResultat(InstanceManager.EntitiesManager.GetResultat(m_currentSommeil, objet));

        m_gameLoopController.OnSelectCard(objet);
    }

    private TypeResultat? SelectResultat(ResultatModel resultat)
    {
        if (resultat == null)
        {
            if (hasResultEvent) _onResult.Invoke("Echec");

            m_gameLoopController.OnSelectResultat(null);
            return null;
        }
        
        if (hasResultEvent) _onResult.Invoke(resultat.typeResultat.PrettyName());

        resultat.UnlockNoteCarnet();

        m_gameLoopController.OnSelectResultat(resultat);

        return resultat.typeResultat;
    }
}