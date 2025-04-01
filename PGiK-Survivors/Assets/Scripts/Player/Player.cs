using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] public ClassSO playerClass;

    private Dictionary<StatType, Stat> playerStats;
    private List<StatModifier> classLvlMods;

    private void Awake() {
        playerStats = playerClass?.GetStats();
        classLvlMods = playerClass?.GetLevelUpMods();    
    }

    public Stat GetStat( StatType type ) {
        if ( playerStats.TryGetValue( type, out Stat stat ) ) return stat;
        else return null;
    }

    public float GetStatTotal( StatType type ) {
        float total = -1;
        
        if ( playerStats.TryGetValue( type, out Stat stat ) ) {
            total = stat.total;            
        }

        return total;
    }
}
