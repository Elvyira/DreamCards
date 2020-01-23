using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum DataType : byte
{
    PlayerData,
    SettingsData
}

public static class SavedDataServices
{
    #region Data

    public struct PlayerData
    {
        public List<ushort> notebookEntries;
    }

    public struct SettingsData
    {
        public byte language;
        public bool mute;
    }

    #endregion

    public const string PlayerDataFileName = "DreamCardsPlayerData";
    public const string SettingsDataFileName = "DreamCardsSettingsData";

    private static string _playerDataPath, _settingsDataPath;

    private static PlayerData _playerData;
    private static SettingsData _settingsData;

#if UNITY_EDITOR
    public static PlayerData EditorPlayerData
    {
        get => _playerData;
        set
        {
            _savePlayer = true;
            _playerData = value;
        }
    }

    public static SettingsData EditorSettingsData
    {
        get => _settingsData;
        set
        {
            _saveSettings = true;
            _settingsData = value;
        }
    }
#endif

    private static bool _savePlayer, _saveSettings;

    #region Static Access

    #region PlayerData

    public static bool IsNoteDiscovered(byte sommeilIndex, TypeNote typeNote)
    {
        var index = NotebookEntries.GetNotebookEntryIndex(sommeilIndex);
        return index != -1 && CheckTypeNote(NotebookEntries[index], (byte) typeNote);
    }

    public static bool DiscoverNote(byte sommeilIndex, TypeNote typeNote)
    {
        var noteMask = (byte) typeNote;
        if (NotebookEntries.Count == 0)
        {
            NotebookEntries.Add(ToNotebookEntry(sommeilIndex, noteMask));
            return true;
        }

        var index = NotebookEntries.GetNotebookEntryIndex(sommeilIndex);
        if (index == -1)
        {
            NotebookEntries.Add(ToNotebookEntry(sommeilIndex, noteMask));
            return true;
        }

        if (CheckTypeNote(NotebookEntries[index], noteMask)) return false;

        NotebookEntries[index] |= noteMask;
        return true;
    }

    private static List<ushort> NotebookEntries
    {
        get => _playerData.notebookEntries;
        set => _playerData.notebookEntries = value;
    }

    #endregion /PlayerData

    #region SettingsData

    public static byte Language
    {
        get => _settingsData.language;
        set
        {
            if (_settingsData.language == value) return;
            _saveSettings = true;
            _settingsData.language = value;
        }
    }

    public static bool Mute
    {
        get => _settingsData.mute;
        set
        {
            if (_settingsData.mute == value) return;
            _saveSettings = true;
            _settingsData.mute = value;
        }
    }

    #endregion /SettingsData

    #endregion /Static Access

    #region Utility

    private const byte INDEX_SHIFT = 8;
    private const ushort NOTES_MASK = 0b0000000000011111;

    public static byte ToBitMask(this byte value) => (byte) (1 << value);

    public static short GetNotebookEntryIndex(this List<ushort> notebookEntries, byte sommeilIndex)
    {
        for (short index = 0; index < notebookEntries.Count; index++)
            if (CheckSommeilIndex(notebookEntries[index], sommeilIndex))
                return index;

        return -1;
    }

    public static byte GetSommeilIndex(ushort notebookEntry) => (byte) (notebookEntry >> INDEX_SHIFT);

    public static ushort ToNotebookEntry(byte sommeilIndex, byte typeNote) =>
        (ushort) ((sommeilIndex << INDEX_SHIFT) + (typeNote & NOTES_MASK));

    public static (byte sommeilIndex, byte typeNote) GetNotebookEntryValues(ushort notebookEntry) =>
        (GetSommeilIndex(notebookEntry), (byte) (notebookEntry & NOTES_MASK));

    private static bool CheckSommeilIndex(ushort notebookEntry, byte sommeilIndex) => (byte) (notebookEntry >> INDEX_SHIFT) == sommeilIndex;

    private static bool CheckTypeNote(ushort notebookEntry, byte typeNote) => (notebookEntry & NOTES_MASK & typeNote) != 0;


    private static string GetPath(DataType dataType)
    {
        switch (dataType)
        {
            case DataType.PlayerData:
                if (string.IsNullOrEmpty(_playerDataPath))
                    _playerDataPath = $"{Path.Combine(Application.persistentDataPath, PlayerDataFileName)}.save";
                return _playerDataPath;
            case DataType.SettingsData:
                if (string.IsNullOrEmpty(_settingsDataPath))
                    _settingsDataPath = $"{Path.Combine(Application.persistentDataPath, SettingsDataFileName)}.save";
                return _settingsDataPath;
            default:
                return null;
        }
    }

    private static void CreateFile(DataType dataType)
    {
        switch (dataType)
        {
            case DataType.PlayerData:
                File.WriteAllText(GetPath(DataType.PlayerData), DataToJson(DataType.PlayerData));
                break;
            case DataType.SettingsData:
                File.WriteAllText(GetPath(DataType.SettingsData), DataToJson(DataType.SettingsData));
                break;
        }
    }

    private static string DataToJson(DataType dataType)
    {
        switch (dataType)
        {
            case DataType.PlayerData:
                return JsonUtility.ToJson(_playerData);
            case DataType.SettingsData:
                return JsonUtility.ToJson(_settingsData);
            default:
                return null;
        }
    }

    private static PlayerData JsonToPlayerData(string json) => JsonUtility.FromJson<PlayerData>(json);

    private static SettingsData JsonToSettingsData(string json) => JsonUtility.FromJson<SettingsData>(json);

    #endregion

    #region Init

    private static void InitManagedPlayerData(bool checkIfNull)
    {
        if (checkIfNull)
        {
            if (NotebookEntries == null)
                NotebookEntries = new List<ushort>();
            return;
        }

        NotebookEntries = new List<ushort>();
    }

    #endregion /Init

    #region Reset

    public static void ResetData(DataType dataType)
    {
        switch (dataType)
        {
            case DataType.PlayerData:
                ResetPlayerData();
                break;
            case DataType.SettingsData:
                ResetSettingsData();
                break;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Custom/Saved Data/Reset Player Data")]
#endif
    public static void ResetPlayerData()
    {
        _playerData = new PlayerData();
        InitManagedPlayerData(false);
        var path = GetPath(DataType.PlayerData);
        if (!File.Exists(path)) return;
        File.Delete(path);
    }

#if UNITY_EDITOR
    [MenuItem("Custom/Saved Data/Reset Settings Data")]
#endif
    public static void ResetSettingsData()
    {
        _settingsData = new SettingsData();
        var path = GetPath(DataType.SettingsData);
        if (!File.Exists(path)) return;
        File.Delete(path);
    }

    #endregion /Reset

    #region Load & Save

    public static void LoadEverything()
    {
        Load(DataType.PlayerData);
        Load(DataType.SettingsData);
    }

    public static void Load(DataType dataType)
    {
        var path = GetPath(dataType);

        if (!File.Exists(path))
        {
            if (dataType == DataType.PlayerData) InitManagedPlayerData(false);

            CreateFile(dataType);
            return;
        }

        try
        {
            switch (dataType)
            {
                case DataType.PlayerData:
                    _playerData = JsonToPlayerData(File.ReadAllText(path));
                    InitManagedPlayerData(true);
                    break;
                case DataType.SettingsData:
                    _settingsData = JsonToSettingsData(File.ReadAllText(path));
                    break;
            }
        }
        catch
        {
            if (dataType == DataType.PlayerData)
                InitManagedPlayerData(true);
            Save(dataType);
        }

#if UNITY_EDITOR
        Debug.Log(path);
        Debug.Log(DataToJson(dataType));
#endif
    }

    public static void SavePlayer() => Save(DataType.PlayerData);

    public static void SavesSettings() => Save(DataType.SettingsData);

    public static void Save(DataType dataType)
    {
        switch (dataType)
        {
            case DataType.PlayerData:
                if (!_savePlayer) return;
                _savePlayer = false;
                break;
            case DataType.SettingsData:
                if (!_saveSettings) return;
                _saveSettings = false;
                break;
        }

        CreateFile(dataType);
    }

    #endregion
}