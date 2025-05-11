using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu( menuName = "Scriptable Objects/Status Effects/Shock" )]
public class ShockEffectSO : StatusEffectSO {
    [Header( "ShockEffect Info" )]
    [field: SerializeField] public float shockDamage;
    [field: SerializeField] public float jumpChance;
    [field: SerializeField] public float jumpRadius;
    [field: SerializeField] public bool isPlayersAttack;


    private EntityController entity;

    private void Awake() {
        isEffectOverTime = true;
    }

    public override void Apply( GameObject target, float effectChance, float tickValue ) {
        if ( target.TryGetComponent( out EntityController entity ) ) {
            timeLeft = durationSeconds;
            isEffectActive = true;

            entity.EntityStatuses.AddEffect( this, effectChance, tickValue );
        }

    }

    public override void UpdateEffect( GameObject target ) {
        SetEntity( target );

        wasActivated = true;

        entity.EntityHealth.Damage( shockDamage + GetTickValue() );

        //jump
        if ( isPlayersAttack ) PerformJump();

        isEffectActive = false;
    }

    private void PerformJump() {
        Collider2D[] enemies = Physics2D.OverlapCircleAll( entity.transform.position, jumpRadius );

        foreach ( Collider2D coll in enemies ) {
            EnemyController enemy = coll.GetComponent<EnemyController>();
            if ( enemy != null ) Jump( enemy.gameObject );
        }
    }

    private void Jump(GameObject target) {
        float chanceRoll = Random.Range( 0.0f, 1.0f );
        if ( chanceRoll <= jumpChance ) {
            Apply( target, 1.0f, GetTickValue() );
        }
    }

    public override void Remove( GameObject target ) {
        SetEntity( target );

        isEffectActive = false;
        wasActivated = false;
        timeLeft = 0.0f;
        tickCooldown = 0.0f;
    }

    private void SetEntity( GameObject target ) {
        if ( entity == null ) {
            entity = target.GetComponent<EntityController>();
        }
    }
}
