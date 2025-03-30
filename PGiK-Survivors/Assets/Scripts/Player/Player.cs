using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player Instance { get; private set; }

    [SerializeField] public Class playerClass;

    private Dictionary<StatType, Stat> playerStats;
    private List<StatModifier> classLvlMods;

    private void Awake() {
        if ( Instance == null ) { Instance = this; }

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

    private void OnDisable() { Instance = null; }

}
