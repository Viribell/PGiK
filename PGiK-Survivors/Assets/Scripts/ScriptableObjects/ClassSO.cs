using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( fileName = "ClassSO" )]
public class ClassSO : ScriptableObject {
    [field: SerializeField] public SerializableDictionary<StatType, float> stats { get; private set; }
    [field: SerializeField] public List<StatModifier> levelUpMods { get; private set; }

    public Dictionary<StatType, Stat> GetStats() {
        if ( stats.Count == 0 ) return null;

        Dictionary<StatType, Stat> statsDict = new Dictionary<StatType, Stat>();

        foreach( KeyValuePair<StatType, float> entry in stats ) {
            Stat stat = new Stat( entry.Value );

            statsDict.Add( entry.Key, stat );
        }

        return statsDict;
    }

    public List<StatModifier> GetLevelUpMods() {
        if ( levelUpMods.Count == 0 ) return null;

        List<StatModifier> modsList = new List<StatModifier>();

        foreach(StatModifier mod in levelUpMods) {
            StatModifier newMod = new StatModifier( mod.value, mod.type, mod.affectedStat );

            modsList.Add( newMod );
        }

        return modsList;
    }
}
