using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu( menuName = "Scriptable Objects/Status Effects/Damage Over Time" )]
public class DoTSO : StatusEffectSO {
    [field: SerializeField] public float damagePerTick;

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

        entity.EntityHealth.Damage( damagePerTick + GetTickValue() );
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
