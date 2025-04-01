using System.Collections;
using System.Collections.Generic;
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

    public void OnLevelUp() {
        foreach( StatModifier mod in classLvlMods ) {
            playerStats[mod.affectedStat].AddMod( mod );
            UpdateEdgeCaseStat( mod.affectedStat );
        }
        //for memory saving, it prolly would be better to just remove those mods from stat, modify value of them X level and then insert again
        //otherwise the list prolly will get quite long for them
    }

    public void TestFunc() {
        Debug.Log( GetStatTotal( StatType.Health ) );
    }

    public void UpdateEdgeCaseStat(StatType stat) {
        switch( stat ) {
            case StatType.PickupRange: { PlayerControl.Instance.PlayerPickup.UpdatePickupRange(); } break;
        }
    }
}
