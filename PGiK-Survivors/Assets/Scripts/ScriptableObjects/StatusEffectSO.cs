using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Undefined,
    RegenUp,
    AttackSpeedUp,
    Invincibility,
    Magnet,
    Burn,
    Shock,
    Freeze,
    Poisoned
}

public abstract class StatusEffectSO : ScriptableObject {
    [Header( "General Info" )]
    [field: SerializeField] public string effectName;
    [field: SerializeField] public EffectType effectType;
    [field: SerializeField] public bool isNegative = false;
    //particle system

    [Header("Duration Info")]
    [field: SerializeField] public bool isEffectOverTime = false;
    [Range( 0.0f, 60.0f )]
    [field: SerializeField] public float tickInterval; // tick per X seconds
    [field: SerializeField][Lock] public float tickCooldown;
    [Range(0.0f, 60.0f)]
    [field: SerializeField] public float durationSeconds;
    [field: SerializeField][Lock] public float timeLeft;

    [HideInInspector] public bool isEffectActive = false;
    [HideInInspector] public bool wasActivated = false;

    public void Tick( GameObject target, float tickAmount ) {
        if ( isEffectActive ) {
            timeLeft -= tickAmount;

            if ( timeLeft <= 0.0f ) isEffectActive = false;

        }

        if( isEffectOverTime ) {

            tickCooldown += tickAmount;
            if ( tickCooldown >= tickInterval ) {
                UpdateEffect( target );
                tickCooldown = 0.0f;
            }

        } else if ( !isEffectOverTime && !wasActivated ) {
            UpdateEffect( target );
        }
    }

    public void Activate() { 
        isEffectActive = true;
        timeLeft = durationSeconds;
    }

    public abstract void Apply( GameObject target, float effectChance = 1.0f );
    public virtual void UpdateEffect( GameObject target ) {}
    public virtual void Remove( GameObject target ) {}
    public virtual bool CanBeRemoved() {
        return !isEffectActive;
    }
}
