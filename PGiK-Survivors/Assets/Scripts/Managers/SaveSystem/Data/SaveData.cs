using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public List<ResourceSaveData> resources;
    public List<NPCSaveData> availableNPC;

    public SaveData() {
        resources = new List<ResourceSaveData>();
        availableNPC = new List<NPCSaveData>();
    }
}
