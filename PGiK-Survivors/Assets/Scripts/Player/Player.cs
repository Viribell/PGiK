using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Player Class Info")]
    [SerializeField] private ClassSO playerClass;

    [Header( "Event Channels" )]
    [SerializeField] private EventChannelSO OnStatChanged;

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

    public void AddStatMod( StatModifier mod ) {
        if ( mod == null ) return;

        GetStat( mod.affectedStat )?.AddMod( mod );

        OnStatChanged?.Raise();
    }

    public void OnLevelUp() {
        foreach( StatModifier mod in classLvlMods ) {
            AddStatMod( mod );
            UpdateEdgeCaseStat( mod.affectedStat ); //depending on how it will change, it may be useless later
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
            case StatType.Health: { PlayerControl.Instance.PlayerHealth.UpdateMaxHealth(); } break;
        }
    }

    public void SetClass(ClassSO playerClass) { this.playerClass = playerClass; }
    public ClassSO GetClass() { return playerClass; }
}
