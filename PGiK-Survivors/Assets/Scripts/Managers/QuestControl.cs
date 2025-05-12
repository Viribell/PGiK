using System.Collections.Generic;
using UnityEngine;

public class QuestControl : MonoBehaviour, IPersistentData {
    public static QuestControl Instance { get; private set; }

    [field: SerializeField] public List<QuestSO> availableQuests { get; private set; }

    [field: SerializeField] private SerializableDictionary<string, QuestSO> quests;

    private List<KillQuestSO> killQuests;
    private List<TrackValueQuestSO> valueQuests;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.LogWarning( "There is more than one instance of QuestControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        InitQuests();

        DontDestroyOnLoad( gameObject );
    }

    public bool IsQuestCompleted(QuestSO quest) {
        if ( quest == null || !quests.Contains(quest.questId) ) return false;

        return quests[quest.questId].isCompleted;
    }

    public bool IsQuestCompleted( string quest ) {
        if ( quest == null || !quests.Contains( quest ) ) return false;

        return quests[quest].isCompleted;
    }

    private void InitQuests() {
        quests = new SerializableDictionary<string, QuestSO>();
        killQuests = new List<KillQuestSO>();
        valueQuests = new List<TrackValueQuestSO>();

        foreach ( QuestSO quest in availableQuests ) {
            quests.Add( quest.questId, Instantiate( quest ) );
        }

        foreach( KeyValuePair<string,QuestSO> quest in quests ) {
            if ( quest.Value is KillQuestSO ) killQuests.Add( ( KillQuestSO )quest.Value );
            else if ( quest.Value is TrackValueQuestSO ) valueQuests.Add( ( TrackValueQuestSO )quest.Value );
        }
    }

    public void UpdateKillQuests(EnemyController enemy) {
        foreach( KillQuestSO quest in killQuests ) {
            quest.OnEnemyKilled( enemy );
        }
    }

    public void UpdateValuesQuests( TrackedType type, float value ) {
        foreach ( TrackValueQuestSO quest in valueQuests ) {
            quest.OnValueChanged( type, value );
        }
    }

    #region SaveLoad
    public void LoadData( SaveData data ) {
        List<QuestSaveData> questDataList = data.quests;

        if ( questDataList == null || questDataList.Count == 0 ) return;

        List<string> tempKeys = new List<string>();

        foreach ( string key in quests.Keys ) {
            tempKeys.Add( key );
        }

        foreach ( string key in tempKeys ) {
            QuestSaveData questSaveData = questDataList.Find( item => string.Equals( item.questId, key ) );

            if ( questSaveData == null ) {
                Debug.Log("No data for this quest!");
                continue;
            }

            quests[key].isCompleted = questSaveData.isCompleted;
            quests[key].currAmount = questSaveData.currAmount;
        }

        tempKeys.Clear();
    }

    public void SaveData( SaveData data ) {
        data.quests.Clear();

        foreach ( KeyValuePair<string, QuestSO> quest in quests ) {
            QuestSaveData questSaveData = new QuestSaveData { questId = quest.Value.questId, isCompleted = quest.Value.isCompleted, currAmount = quest.Value.currAmount };
            data.quests.Add( questSaveData );
        }
    }
    #endregion
}
