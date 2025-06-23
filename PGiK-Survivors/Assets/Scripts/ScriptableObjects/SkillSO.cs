using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SkillType {
    Meteor,
    FlameArrows,
    Thunderstorm,
    MagicShell,
    Plague,
    CatsPaw,
    IceChasm,
    PoisonousNeedles,
    IceBall,
    Flamethrower
}

[CreateAssetMenu( menuName = "Scriptable Objects/Skill" )]
public class SkillSO : ScriptableObject{
    [Header( "Skill Info" )]
    [field: SerializeField] public string skillName;
    [field: SerializeField] public SkillType skillType;
    [field: SerializeField] public CardSetSO SkillCards { get; private set; }

    [Header( "Skill Attack Info" )]
    [field: SerializeField] public SkillObject skillObjectPrefab;
    [field: SerializeField] public float spreadAngle = 30.0f;
    [field: SerializeField] public StatusEffectSO statusEffect;

    [Header( "Skill Stats Info" )]
    [field: SerializeField] public SerializableDictionary<StatType, float> stats = new SerializableDictionary<StatType, float>();

    [Header( "Skill Price" )]
    [field: SerializeField] public SerializableDictionary<ResourceSO, int> price = new SerializableDictionary<ResourceSO, int>();

    private bool isEffectActive = false;
    private float timeLeft;

    private Dictionary<StatType, Stat> workStats;

    public void Init() {
        workStats = GetStats();
    }

    #region UpdateRelated

    public void Tick( EntityController source, float tickAmount ) {
        if ( isEffectActive ) {
            timeLeft -= tickAmount;

            if ( timeLeft <= 0.0f ) {
                UpdateEffect( source );
                Restart();
            }

        }
    }

    public void Activate() {
        isEffectActive = true;
        timeLeft = GetStatTotal( StatType.Cooldown );
    }

    public void Restart() {
        timeLeft = GetStatTotal( StatType.Cooldown );
    }

    public void UpdateEffect( EntityController source ) {
        Debug.Log( "Skill " + skillType.ToString() + " was used." );

        SkillObject skillObject = Instantiate( skillObjectPrefab, source.transform );

        skillObject.Init( source, source.transform.position , this );

    }
    #endregion

    #region StatRelated
    public Stat GetStat( StatType type ) {
        if ( workStats.TryGetValue( type, out Stat stat ) ) return stat;
        else return null;
    }

    public float GetStatTotal( StatType type ) {
        float total = 0.0f;

        if ( workStats.TryGetValue( type, out Stat stat ) ) {
            total = stat.total;
        }

        return total;
    }

    public float GetStatTotal( EffectType type ) {
        float total = 0.0f;
        StatType effectChance = StatType.Undefined;

        switch ( type ) {
            case EffectType.Burn: { effectChance = StatType.BurnChance; } break;
            case EffectType.Freeze: { effectChance = StatType.FreezeChance; } break;
            case EffectType.Shock: { effectChance = StatType.ShockChance; } break;
            case EffectType.Poisoned: { effectChance = StatType.PoisonChance; } break;
        }

        if ( workStats.TryGetValue( effectChance, out Stat stat ) ) {
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

        if ( workStats.TryGetValue( effectDamage, out Stat stat ) ) {
            total = stat.total;
        }

        return total;
    }

    public void AddStatMod( StatModifier mod, StatModHandlingOptions options = StatModHandlingOptions.None, float multFactor = 1.0f ) {
        if ( mod == null ) return;

        Stat currStat = GetStat( mod.affectedStat );

        if ( !( bool )currStat?.HasMod( mod ) || ( options & StatModHandlingOptions.NoDuplicateMods ) == 0 ) {
            currStat?.AddMod( mod );

        } else {
            if ( ( options & StatModHandlingOptions.ModAdditiveIncrease ) != 0 ) currStat?.ModifyModAdd( mod, mod.DefaultValue );
            else if ( ( options & StatModHandlingOptions.ModFactorMultiply ) != 0 ) currStat?.ModifyModMult( mod, multFactor );
        }
    }

    public void AddStatMod( List<StatModifier> mods, StatModHandlingOptions options = StatModHandlingOptions.None, float multFactor = 1.0f ) {
        if ( mods == null || mods.Count == 0 ) return;

        foreach ( StatModifier mod in mods ) {
            Stat currStat = GetStat( mod.affectedStat );

            if ( !( bool )currStat?.HasMod( mod ) || ( options & StatModHandlingOptions.NoDuplicateMods ) == 0 ) {
                currStat?.AddMod( mod );

            } else {
                if ( ( options & StatModHandlingOptions.ModAdditiveIncrease ) != 0 ) currStat?.ModifyModAdd( mod, mod.DefaultValue );
                else if ( ( options & StatModHandlingOptions.ModFactorMultiply ) != 0 ) currStat?.ModifyModMult( mod, multFactor );

            }

        }
    }

    public void UpdateStat( StatType type ) {
        if ( workStats.TryGetValue( type, out Stat stat ) ) {
            stat.Recalculate();
        }
    }

    public Dictionary<StatType, Stat> GetStats() {
        if ( stats.Count == 0 ) return null;

        Dictionary<StatType, Stat> statsDict = new Dictionary<StatType, Stat>();

        foreach ( KeyValuePair<StatType, float> entry in stats ) {
            Stat stat = new Stat( entry.Value );

            statsDict.Add( entry.Key, stat );
        }

        return statsDict;
    }
    #endregion
}
