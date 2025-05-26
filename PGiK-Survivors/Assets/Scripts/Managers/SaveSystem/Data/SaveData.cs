using System.Collections.Generic;

[System.Serializable]
public class SaveData {
    public List<ResourceSaveData> resources;
    public List<NPCSaveData> availableNPC;
    public List<QuestSaveData> quests;
    public List<LevelSaveData> levels;
    public AudioSaveData audioData;
    public List<UpgradeSaveData> upgrades;

    public SaveData() {
        resources = new List<ResourceSaveData>();
        availableNPC = new List<NPCSaveData>();
        quests = new List<QuestSaveData>();
        upgrades = new List<UpgradeSaveData>();

        CreateLevels();

        audioData = new AudioSaveData();
    }

    public void CreateLevels() {
        levels = new List<LevelSaveData>();

        levels.Add( new LevelSaveData( "Forest" ) );
        levels.Add( new LevelSaveData( "Cemetery" ) );
        levels.Add( new LevelSaveData( "Desert" ) );
        levels.Add( new LevelSaveData( "Tundra" ) );
        levels.Add( new LevelSaveData( "Cave" ) );
    }
}
