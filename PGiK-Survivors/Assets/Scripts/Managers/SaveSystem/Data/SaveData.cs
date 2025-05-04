using System.Collections.Generic;

[System.Serializable]
public class SaveData {
    public List<ResourceSaveData> resources;
    public List<NPCSaveData> availableNPC;
    public List<QuestSaveData> quests;
    public AudioSaveData audioData;

    public SaveData() {
        resources = new List<ResourceSaveData>();
        availableNPC = new List<NPCSaveData>();
        quests = new List<QuestSaveData>();

        audioData = new AudioSaveData();
    }
}
