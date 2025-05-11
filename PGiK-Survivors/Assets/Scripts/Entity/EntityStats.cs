using System.Collections.Generic;
using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Flags]
public enum StatModHandlingOptions {
    None = 0,
    NoDuplicateMods = 1,
    ModAdditiveIncrease = 2,
    ModFactorMultiply = 3,
    NoDuplicateModAdd = NoDuplicateMods | ModAdditiveIncrease,
    NoDuplicateModMult = NoDuplicateMods | ModFactorMultiply
}

public class EntityStats : MonoBehaviour, IEntityComponent {
    [Header( "Event Channels" )]
    [SerializeField] private EventChannelSO OnStatChanged;

    private EntityController entityController;

    private Dictionary<StatType, Stat> entityStats;

#if UNITY_EDITOR
    [HideInInspector] public Dictionary<StatType, Stat> Editor_Stats { get { return entityStats; } }
#endif

    public Stat GetStat( StatType type ) {
        if ( entityStats.TryGetValue( type, out Stat stat ) ) return stat;
        else return null;
    }

    public void UpdateStat( StatType type ) {
        if ( entityStats.TryGetValue( type, out Stat stat ) ) {
            stat.Recalculate();
        }
    }

    public float GetStatTotal( StatType type ) {
        float total = 0.0f;

        if ( entityStats.TryGetValue( type, out Stat stat ) ) {
            total = stat.total;
        }

        return total;
    }

    public float GetStatTotal( EffectType type ) {
        float total = 0.0f;
        StatType effectChance = StatType.Undefined;

        switch( type ) {
            case EffectType.Burn: { effectChance = StatType.BurnChance; } break;
            case EffectType.Freeze: { effectChance = StatType.FreezeChance; } break;
            case EffectType.Shock: { effectChance = StatType.ShockChance; } break;
            case EffectType.Poisoned: { effectChance = StatType.PoisonChance; } break;
        }

        if ( entityStats.TryGetValue( effectChance, out Stat stat ) ) {
            total = stat.total;

            total += total * GetStatTotal( StatType.EffectHitChance );
        }

        return total;
    }

    public float GetEffectDamage( EffectType type ) {
        float total = 0.0f;
        StatType effectDamage = StatType.Undefined;

        switch ( type ) {
            case EffectType.Burn: { effectDamage = StatType.BurnDmg; } break;
            case EffectType.Shock: { effectDamage = StatType.ShockDmg; } break;
            case EffectType.Poisoned: { effectDamage = StatType.PoisonDmg; } break;
        }

        if ( entityStats.TryGetValue( effectDamage, out Stat stat ) ) {
            total = stat.total;
        }

        return total;
    }

    public void AddStatMod( StatModifier mod, StatModHandlingOptions options = StatModHandlingOptions.None, float multFactor = 1.0f) {
        if ( mod == null ) return;

        Stat currStat = GetStat( mod.affectedStat );

        if ( !( bool )currStat?.HasMod( mod ) || ( options & StatModHandlingOptions.NoDuplicateMods ) == 0) {
            currStat?.AddMod( mod );

        } else {
            if ( ( options & StatModHandlingOptions.ModAdditiveIncrease ) != 0 ) currStat?.ModifyModAdd( mod, mod.DefaultValue );
            else if ( ( options & StatModHandlingOptions.ModFactorMultiply ) != 0 ) currStat?.ModifyModMult( mod, multFactor );
        }

        OnStatChanged?.Raise();
    }

    public void AddStatMod( List<StatModifier> mods, StatModHandlingOptions options = StatModHandlingOptions.None, float multFactor = 1.0f ) {
        if ( mods == null || mods.Count == 0 ) return;

        foreach(StatModifier mod in mods) {
            Stat currStat = GetStat( mod.affectedStat );

            if ( !( bool )currStat?.HasMod( mod ) || ( options & StatModHandlingOptions.NoDuplicateMods ) == 0 ) {
                currStat?.AddMod( mod );

            } else {
                if ( ( options & StatModHandlingOptions.ModAdditiveIncrease ) != 0 ) currStat?.ModifyModAdd( mod, mod.DefaultValue );
                else if ( ( options & StatModHandlingOptions.ModFactorMultiply ) != 0 ) currStat?.ModifyModMult( mod, multFactor );

            }

        }

        OnStatChanged?.Raise();
    }

    public void LoadEntityController( EntityController controller ) {
        entityController = controller;

        entityStats = entityController.EntityData?.GetStats();
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(EntityStats))]
public class EntityStatsEditor : Editor {
    private EntityStats t;

    private bool foldOut;

    private void OnEnable() {
        t = ( EntityStats )target;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        base.OnInspectorGUI();

        foldOut = EditorGUILayout.BeginFoldoutHeaderGroup( foldOut, "Current Stats" );

        if ( foldOut && t.Editor_Stats != null) {
            PrintStats();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();


        serializedObject.ApplyModifiedProperties();
    }

    private void PrintStats() {
        foreach (KeyValuePair<StatType, Stat> entry in t.Editor_Stats) {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField( entry.Key.ToString() );
            EditorGUILayout.LabelField( entry.Value.Editor_Total.ToString() );

            EditorGUILayout.EndHorizontal();
        }
    }
}

#endif
