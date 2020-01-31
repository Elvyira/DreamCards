using UnityEngine;

public class DiaryController : MonoBehaviour
{
    [SerializeField] private DiaryPageController _leftPage, _rightPage;

    private int m_noteCount;
    private int m_index;
    private NoteCarnet m_leftNote, m_rightNote;

    public void Init()
    {
        m_noteCount = InstanceManager.EntitiesManager.GetUnlockedNotesCarnet().Length;
        m_index = 0;
        ShowNotes();
    }

    public void NextPage()
    {
        if (m_index >= m_noteCount - 2) return;
        m_index += 2;
        ShowNotes();
    }

    public void PreviousPage()
    {
        if (m_index == 0) return;
        m_index -= 2;
        ShowNotes();
    }

    private void ShowNotes()
    {
        var noteCarnets = InstanceManager.EntitiesManager.GetUnlockedNotesCarnet();

        if (m_noteCount == 0)
        {
            _leftPage.ResetNote();
            _rightPage.ResetNote();
            
            InstanceManager.AudioManager.PlayTurnPage();
            return;
        }

        InstanceManager.AudioManager.PlayTurnPage();
        
        _leftPage.SetNote(noteCarnets[m_index]);
        if (m_index < m_noteCount - 1)
            _rightPage.SetNote(noteCarnets[m_index + 1]);
        else
            _rightPage.ResetNote();
    }
}