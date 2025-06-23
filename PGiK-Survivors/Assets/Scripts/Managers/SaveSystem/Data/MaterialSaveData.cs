using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialSaveData {
    public string name;
    public int currentLevel;
    public int maxLevels;

    public MaterialSaveData() {
        name = "";
        currentLevel = 0;
        maxLevels = 1;
    }
}
