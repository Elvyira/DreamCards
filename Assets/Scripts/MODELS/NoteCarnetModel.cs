#if UNITY_EDITOR
using System.Linq;
#endif
using NaughtierAttributes;
using UnityEngine;

public enum TypeNote : byte
{
    ReussiteCritique,
    Reussite1,
    Reussite2,
    EchecCritique1,
    EchecCritique2
}

[CreateAssetMenu(menuName = "Entity/Note Carnet", fileName = "Note Carnet")]
public class NoteCarnetModel : ScriptableObject
{
    [ResizableTextArea] public string note;
    [FoldGroup("Debug"), ReadOnly] public TypeNote typeNote;
    [FoldGroup("Debug"), ReadOnly] public SommeilModel sommeil;

    #region Editor

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sommeil == null) return;

        switch (typeNote)
        {
            case TypeNote.Reussite1:
                if (IsThereNotesWithSameSommeilAndType())
                    typeNote = TypeNote.Reussite2;
                break;
            case TypeNote.EchecCritique1:
                if (IsThereNotesWithSameSommeilAndType())
                    typeNote = TypeNote.EchecCritique2;
                break;
        }
    }

    private bool IsThereNotesWithSameSommeilAndType() =>
        sommeil != null && EditModeUtility.FindAssetsOfType<NoteCarnetModel>()
            .Where(x => x.sommeil == sommeil && x.GetInstanceID() != GetInstanceID())
            .Any(x => x.typeNote == typeNote);
#endif

    #endregion /Editor
}