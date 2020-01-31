using TMPro;
using UnityEngine;

public class DiaryPageController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sommeilText, _objetText, _noteText;

    public void SetNote(NoteCarnet noteCarnet)
    {
        _sommeilText.text = noteCarnet.sommeil;
        _objetText.text = noteCarnet.objet;
        _noteText.text = noteCarnet.note;
    }

    public void ResetNote()
    {
        _sommeilText.text = "";
        _objetText.text = "";
        _noteText.text = "";
    }
}
