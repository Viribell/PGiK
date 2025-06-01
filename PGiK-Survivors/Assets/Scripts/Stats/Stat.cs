using System;
using System.Collections.Generic;

public enum StatType {
    Health,             // flat
    Regeneration,       // flat per s
    Defense,            // flat
    CritChance,         // %
    CritBonus,          // %
    Speed,              // unit per s
    PickupRange,        // magnetic pickup range, flat number
    AttackSpeed,        // attack per s
    Strikes,            // 1.0
    AttackSize,         // 100% is normal
    AttackRange,        // flat unit
    Damage,             // output dmg consisting of ...
    BurnDmg,            // flat dmg per s
    PoisonDmg,          // flat dmg per s
    ShockDmg,           // instancja
    ExpGain,            // 100%
    BurnChance,
    PoisonChance,
    ShockChance,
    FreezeChance,
    EffectHitChance,    // %
    EffectResistance,    // %
    Undefined,
    Lifetime,
    Cooldown
}

//currently 22 stats 

//% should be written as values 0.

[Serializable]
public class Stat {
    public float baseValue;

    public float total {
        get {
            if( HasModChanged || baseValue != lastBaseValue ) {
                lastBaseValue = baseValue;
                totalValue = CalculateTotal();
                HasModChanged = false;
            }

            return totalValue;
        }
    }

    private bool HasModChanged = true;
    private float totalValue = -1;
    private float lastBaseValue;

#if UNITY_EDITOR
    public float Editor_Total { get {
            if ( totalValue == -1 ) return baseValue;
            else return totalValue; 
        } 
    }
#endif

    private List<StatModifier> statModifiers;

    public Stat() {
        statModifiers = new List<StatModifier>();
    }

    public Stat(float baseValue) : this() {
        lastBaseValue = this.baseValue = baseValue;
    }

    public void Recalculate() {
        if ( HasModChanged || baseValue != lastBaseValue ) {
            lastBaseValue = baseValue;
            totalValue = CalculateTotal();
            HasModChanged = false;
        }
    }

    public void AddMod(StatModifier mod) {
        HasModChanged = true;
        statModifiers.Add( mod );
        statModifiers.Sort( CompareMods );
    }

    public bool RemoveMod(StatModifier mod) {
        
        if (statModifiers.Remove( mod ) ) {
            HasModChanged = true;
            return true;
        }

        return false;
    }

    public bool HasMod( StatModifier mod ) {
        if ( statModifiers.IndexOf( mod ) < 0 ) return false;
        else return true;
    }

    public bool ModifyModAdd( StatModifier mod, float value ) {
        if ( !HasMod( mod ) ) return false;

        mod.value += value;
        HasModChanged = true;

        return true;
    }

    public bool ModifyModMult( StatModifier mod, float value ) {
        if ( !HasMod( mod ) ) return false;

        mod.value *= value;
        HasModChanged = true;

        return true;
    }

    private int CompareMods(StatModifier a, StatModifier b) {
        if ( a.applyOrder < b.applyOrder ) return -1;
        else if ( a.applyOrder > b.applyOrder ) return 1;
        else return 0;
    }

    private float CalculateTotal() {
        float localTotal = baseValue;
        float percentAddValue = 0;

        //za³o¿one ¿e jest posortowana lista!

        for( int i = 0; i < statModifiers.Count; i++ ) {
            StatModifier mod = statModifiers[i];

            if( mod.type == StatModType.Flat ) {
                localTotal += mod.value;

            } else if( mod.type == StatModType.PercentAdd ) {
                percentAddValue += mod.value;

                if( i + 1 >= statModifiers.Count || statModifiers[i + 1].type != StatModType.PercentAdd ) {
                    localTotal *= 1 + percentAddValue;
                    percentAddValue = 0;
                }

            } else if ( mod.type == StatModType.PercentMult ) {
                localTotal *= 1 + mod.value;

            } else if ( mod.type == StatModType.BasePercentAdd ) {
                percentAddValue += mod.value;

                if ( i + 1 >= statModifiers.Count || statModifiers[i + 1].type != StatModType.BasePercentAdd ) {
                    localTotal += baseValue * percentAddValue;
                    percentAddValue = 0;
                }

            } else if ( mod.type == StatModType.BasePercentMult ) {
                localTotal += baseValue * mod.value;

            }
        }


        return (float)Math.Round( localTotal, 4);
    }


    public int ModsCount() { return statModifiers.Count; }
}
