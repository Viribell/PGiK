using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum EntityType {
    Player,
    Enemy
}

public abstract class EntitySO : ScriptableObject {
    [Header( "Entity Info" )]
    [field: SerializeField] public string entityName;
    [field: SerializeField] public Sprite sprite;

    [Header( "Entity Stats Info" )]
    [field: SerializeField] public StatsConfigSO statsTemplate;
    [field: SerializeField] public SerializableDictionary<StatType, float> stats = new SerializableDictionary<StatType, float>();

    public Dictionary<StatType, Stat> GetStats() {
        if ( stats.Count == 0 ) return null;

        Dictionary<StatType, Stat> statsDict = new Dictionary<StatType, Stat>();

        foreach ( KeyValuePair<StatType, float> entry in stats ) {
            Stat stat = new Stat( entry.Value );

            statsDict.Add( entry.Key, stat );
        }

        return statsDict;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(EntitySO), true)]
public class EntitySOEditor: Editor {
    private EntitySO entityObject;

    private SerializedProperty statsTemplate;

    private void OnEnable() {
        entityObject = (EntitySO)target;

        statsTemplate = serializedObject.FindProperty( "statsTemplate" );
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        base.OnInspectorGUI();

        EditorGUILayout.Space( 20.0f );

        DrawCall();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCall() {
        if ( GUILayout.Button( "Load Stats Template" ) ) {
            PopulateStats();
        }
    }

    private void PopulateStats() {
        if ( entityObject.stats == null ) {
            entityObject.stats = new SerializableDictionary<StatType, float>();
        } else {
            entityObject.stats.Clear();
        }

        foreach ( StatType statType in entityObject.statsTemplate.usedStats ) {
            if ( !entityObject.stats.ContainsKey( statType ) ) entityObject.stats.Add( statType, 0.0f );
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
