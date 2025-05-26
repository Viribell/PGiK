using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType {
    Compass,
    PlusLife,
    PlusCard,
    SkillBook,

    IncHP,
    IncDef,
    IncDMG,
    IncBurnDMG,
    IncPoisonDMG,
    IncShockDMG,
    IncAtkSpd,
    IncStrikes,
    IncAtkRange,
    IncAtkSize,
    IncSpeed,
    IncEffectChance,
    IncEffectResist,
    IncPickupRange,
    IncRegen,
    IncXPGain
}

[CreateAssetMenu( menuName = "Scriptable Objects/Upgrades/Upgrade" )]
public class UpgradeSO : ScriptableObject {
    [Header("Basic Info")]
    [field: SerializeField] public UpgradeType upgrade;
    [field: SerializeField] public int maxLevels;
    [field: SerializeField] public StatModifier upgradeMod;

    [Header( "Price Info" )]
    [field: SerializeField] public int basicPrice;
    [field: SerializeField] public float priceIncreasePercentage;
}
