#if UNITY_EDITOR
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;

namespace MightyAttributes.Editor
{
    public class MightyAutoValues : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!MightySettingsServices.AutoValuesOnBuild) return;
            ApplyAutoValues();
        }

        [DidReloadScripts]
        private static void OnScriptLoaded() => EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;

        private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode && MightySettingsServices.AutoValuesOnPlay) ApplyAutoValues();
        }

        [MenuItem("[Mighty]Attributes/Apply Auto Values", priority = 2)]
        public static async void ApplyAutoValuesAsync()
        {
            AutoValuesWindowUtility.Open();
            await Task.Delay(50);

            var mightyEditors = MightyEditorUtility.GetMightyEditors().ToArray();
            AutoValuesWindowUtility.DisplayCount(mightyEditors.Length);

            for (var i = 0; i < mightyEditors.Length; i++)
            {
                AutoValuesWindowUtility.SetIndex(i);
                await Task.Yield();
                mightyEditors[i].ApplyAutoValues();
            }

            AutoValuesWindowUtility.Close();
            
            EditorDrawUtility.MightyDebug("Auto Values Applied");
        }

        public static void ApplyAutoValues()
        {
            foreach (var script in SerializedPropertyUtility.FindAllObjects<MonoBehaviour>())
                if (script.CreateEditor(out var mightyEditor))
                    mightyEditor.ApplyAutoValues();
            
            EditorDrawUtility.MightyDebug("Auto Values Applied");
        }
    }

    public static class MightySettingsServices
    {
        #region Params

        private struct MightySettingsParam<T>
        {
            private readonly string m_paramName;
            private readonly T m_defaultValue;

            public T Value
            {
                get => PlayerPrefsUtilities.GetPlayerPref(m_paramName, m_defaultValue);
                set => PlayerPrefsUtilities.SetPlayerPref(m_paramName, value);
            }

            public MightySettingsParam(string name, T defaultValue)
            {
                m_paramName = name;
                m_defaultValue = defaultValue;
            }
        }

        private static MightySettingsParam<bool> m_activated = new MightySettingsParam<bool>("Activated", true);
        private static MightySettingsParam<bool> m_autoValuesOnPlay = new MightySettingsParam<bool>("AutoValuesOnPlay", true);
        private static MightySettingsParam<bool> m_autoValuesOnBuild = new MightySettingsParam<bool>("AutoValuesOnBuild", true);

        #endregion /Params

        #region Properties

        public static bool Activated
        {
            get => m_activated.Value;
            set => m_activated.Value = value;
        }

        public static bool AutoValuesOnPlay
        {
            get => m_autoValuesOnPlay.Value;
            set => m_autoValuesOnPlay.Value = value;
        }

        public static bool AutoValuesOnBuild
        {
            get => m_autoValuesOnBuild.Value;
            set => m_autoValuesOnBuild.Value = value;
        }

        #endregion /Properties
    }
}
#endif