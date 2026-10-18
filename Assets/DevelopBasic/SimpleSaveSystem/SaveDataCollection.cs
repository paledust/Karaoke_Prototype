using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSaveSystem
{
    [Serializable]
    public class GlobalSaveData
    {
        public string currentScene = string.Empty;
        public int MaxLevelIndex = 0;
    }
    [Serializable]
    public class SettingSaveData{}

    [Serializable]
    public class SaveData
    {
        public List<string> learnedWhisper = new List<string>();
    }
}