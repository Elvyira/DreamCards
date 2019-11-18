#if UNITY_EDITOR
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class BaseSavedDataEditorWindows : BaseEditorWindow
{
    private Vector2 m_scrollPosition;

    protected void DrawAllData(Action drawAction) => m_scrollPosition = DrawUtility.DrawScrollView(m_scrollPosition, drawAction);

    protected void DrawData(DataType dataType, ref bool unfold, Action drawAction)
    {
        var label = Regex.Replace(dataType.ToString(), "([a-z])([A-Z])", "$1 $2");
        var lblSize = label.Length;
        var localUnfold = unfold;
        DrawUtility.DrawHorizontal(() =>
        {
            GUILayout.Space(5);
            DrawUtility.DrawVertical(() =>
            {
                GUILayout.Space(5);
                DrawUtility.DrawFoldableBox(label, ref localUnfold,
                    () =>
                    {
                        DrawUtility.DrawHorizontalLine();
                        DrawUtility.DrawHorizontal(() =>
                        {
                            GUILayout.Space(5);
                            DrawUtility.DrawButton(IconName.SAVE, $"Save {label}",
                                () => DrawUtility.YesNoDialog($"Save {label}", $"Are you sure to overwrite {label}?",
                                    () => SavedDataServices.Save(dataType)),
                                GUILayout.Height(35), GUILayout.Width(35 + (9 + lblSize) * 5));

                            DrawUtility.DrawButton(IconName.REFRESH, $"Reload {label}",
                                () => SavedDataServices.Load(dataType), GUILayout.Height(35),
                                GUILayout.Width(35 + (11 + lblSize) * 5));

                            DrawUtility.DrawButton(IconName.TRASH, $"Reset {label}",
                                () => DrawUtility.YesNoDialog($"Reset {label}", $"Are you sure to reset {label}?",
                                    () => SavedDataServices.ResetData(dataType)),
                                GUILayout.Height(35), GUILayout.Width(35 + (10 + lblSize) * 5));
                        });
                        GUILayout.Space(5);
                        DrawUtility.DrawHorizontalLine();

                        drawAction();

                        GUILayout.Space(5);
                    }, Indent.Nothing);
            });
        });
        unfold = localUnfold;
    }
    
    protected bool DrawAddButton() => GUILayout.Button(DrawUtility.DrawIcon(IconName.PLUS), GUILayout.Height(35), GUILayout.Width(35));
    protected bool DrawRemoveButton() => GUILayout.Button(DrawUtility.DrawIcon(IconName.MINUS), GUILayout.Height(35), GUILayout.Width(35));
}
#endif