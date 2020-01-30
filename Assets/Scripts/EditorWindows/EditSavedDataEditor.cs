using System.Text.RegularExpressions;
#if UNITY_EDITOR
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditSavedDataEditor : BaseSavedDataEditorWindows
{
    private bool m_unfoldPlayerData,
        m_unfoldSettingsData,
        m_unfoldNotebookEntries;

    [MenuItem("Custom/Saved Data/Edit Saved Data")]
    private static void Init()
    {
        SavedDataServices.LoadEverything();
        GetWindow<EditSavedDataEditor>().Show();
    }

    protected override void InitWindow() => Init();

    private void OnGUI()
    {
        titleContent = new GUIContent(DrawUtility.DrawIcon("SaveActive"))
        {
            text = " Edit Saved Data"
        };
        minSize = new Vector2(400, 65);
        DrawScriptFeatures();
        GUILayout.Space(5);
        DrawAllData(() =>
        {
            DrawPlayerData();
            DrawSettingsData();
        });
    }

    private void DrawPlayerData()
    {
        DrawData(DataType.PlayerData, ref m_unfoldPlayerData, () =>
        {
            var playerData = SavedDataServices.EditorPlayerData;
            DrawNotebookEntries(InstanceManager.EntitiesManager, ref playerData);
            SavedDataServices.EditorPlayerData = playerData;
        });
    }

    private void DrawSettingsData()
    {
        DrawData(DataType.SettingsData, ref m_unfoldSettingsData, () =>
        {
            var settings = SavedDataServices.EditorSettingsData;
//            DrawLanguage(ref settings);
            DrawMute(ref settings);
            SavedDataServices.EditorSettingsData = settings;
        });
    }

    #region Player Data

    private void DrawNotebookEntries(EntitiesManager entitiesManager, ref SavedDataServices.PlayerData playerData)
    {
        if (playerData.notebookEntries == null)
            playerData.notebookEntries = new List<ushort>();

        var data = playerData;
        var sommeilEntities = entitiesManager.SommeilEntities;
        DrawUtility.DrawListFolder("Notes Carnet", ref m_unfoldNotebookEntries, () =>
        {
            var canAddItem = false;
            byte sommeilIdx = 0;
            for (; sommeilIdx < sommeilEntities.Length; sommeilIdx++)
            {
                if (data.notebookEntries.GetNotebookEntryIndex(sommeilIdx) != -1 ||
                    entitiesManager.GetNotesCarnetBySommeil(sommeilEntities[sommeilIdx]).Length == 0) continue;
                canAddItem = true;
                break;
            }

            DrawUtility.BeginGUILayoutIndent();
            DrawUtility.DrawVertical(() =>
            {
                for (byte i = 0; i < data.notebookEntries.Count; i++)
                {
                    DrawUtility.DrawHorizontal(() =>
                    {
                        GUILayout.BeginVertical(GUILayout.Height(50));
                        GUILayout.FlexibleSpace();
                        if (DrawRemoveButton())
                        {
                            data.notebookEntries.RemoveAt(i);
                            return;
                        }

                        GUILayout.FlexibleSpace();
                        GUILayout.EndVertical();

                        DrawUtility.DrawVertical(() =>
                        {
                            var (sommeilIndex, typeNote) = SavedDataServices.GetNotebookEntryValues(data.notebookEntries[i]);
                            EditorGUI.indentLevel--;
                            var labelWidth = EditorGUIUtility.labelWidth;
                            var fieldWith = EditorGUIUtility.fieldWidth;
                            EditorGUIUtility.labelWidth = 100;
                            EditorGUIUtility.fieldWidth = 25;
                            DrawUtility.DrawHorizontal(() =>
                            {
                                DrawUtility.DrawVertical(() =>
                                {
                                    GUI.enabled = canAddItem;
                                    sommeilIndex = (byte) EditorGUILayout.IntSlider("Index Sommeil", sommeilIndex, 0,
                                        sommeilEntities.Length - 1);
                                    EditorGUILayout.LabelField("Sommeil", sommeilEntities[sommeilIndex].nom, EditorStyles.boldLabel);
                                    GUI.enabled = true;
                                });
                            });

                            var notes = entitiesManager.GetNotesCarnetBySommeil(sommeilEntities[sommeilIndex]);
                            var noteIndexes = 0;

                            var everything = true;

                            for (byte j = 0; j < notes.Length; j++)
                                if ((typeNote & (byte) notes[j].typeNote) != 0)
                                    noteIndexes |= j.ToBitMask();
                                else
                                    everything = false;

                            if (notes.Length > 0)
                            {
                                var noteNames = 
                                    notes.Select(x => $@"{x.objet}            ________            {x.typeNote.PrettyName()}").ToArray();
                                
                                DrawUtility.DrawHorizontal(() =>
                                {
                                    EditorGUIUtility.fieldWidth = fieldWith;
                                    noteIndexes = (byte) EditorGUILayout.MaskField("Notes", everything ? -1 : noteIndexes, noteNames);

                                    typeNote = 0;
                                    for (byte j = 0; j < notes.Length; j++)
                                        if ((noteIndexes & j.ToBitMask()) != 0)
                                            typeNote |= (byte) notes[j].typeNote;

                                    GUI.enabled = false;
                                    EditorGUILayout.LabelField("Notebook Entry", data.notebookEntries[i].ToString());
                                    GUI.enabled = true;
                                    EditorGUIUtility.labelWidth = labelWidth;
                                });
                                EditorGUI.indentLevel++;
                            }

                            var entryIndex = data.notebookEntries.GetNotebookEntryIndex(sommeilIndex);

                            if (entryIndex != -1 && entryIndex != i)
                                sommeilIndex = SavedDataServices.GetSommeilIndex(data.notebookEntries[i]);

                            data.notebookEntries[i] = SavedDataServices.ToNotebookEntry(sommeilIndex, typeNote);
                        });
                    }, GUI.skin.box);
                }

                GUI.enabled = canAddItem;
                if (DrawAddButton()) data.notebookEntries.Add(SavedDataServices.ToNotebookEntry(sommeilIdx, 0));
                GUI.enabled = true;
            });
            DrawUtility.EndGUILayoutIndent();
        });
        playerData = data;
    }

    #endregion /Player Data

    #region Settings Data

//    private void DrawLanguage(ref SavedDataServices.SettingsData settingsData) => settingsData.language =
//        (byte) (Language) EditorGUILayout.EnumPopup("Language", (Language) settingsData.language);

    private void DrawMute(ref SavedDataServices.SettingsData settingsData) =>
        settingsData.mute = EditorGUILayout.Toggle("Mute", settingsData.mute);

    #endregion /Settings Data
}
#endif