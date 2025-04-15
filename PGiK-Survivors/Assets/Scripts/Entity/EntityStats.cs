using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour, IEntityComponent {
    [Header( "Event Channels" )]
    [SerializeField] private EventChannelSO OnStatChanged;

    private EntityController entityController;

    private Dictionary<StatType, Stat> entityStats;

    public Stat GetStat( StatType type ) {
        if ( entityStats.TryGetValue( type, out Stat stat ) ) return stat;
        else return null;
    }

    public float GetStatTotal( StatType type ) {
        float total = -1;

        if ( entityStats.TryGetValue( type, out Stat stat ) ) {
            total = stat.total;
        }

        return total;
    }

    public void AddStatMod( StatModifier mod ) {
        if ( mod == null ) return;

        GetStat( mod.affectedStat )?.AddMod( mod );

        OnStatChanged?.Raise();

        //Add AddStatModList to not call OnStatChanged every stat but every list of stat mods
    }



    public void TestFunc() {
        Debug.Log( GetStatTotal( StatType.Health ) );
    }

    public void LoadEntityController( EntityController controller ) {
        entityController = controller;

        entityStats = entityController.EntityData?.GetStats();
    }
}
