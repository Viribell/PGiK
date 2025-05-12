using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Status Effects/Invincibility" )]
public class InvincibilitySO : StatusEffectSO {
    private EntityController entity;

    private void Awake() {
        isEffectOverTime = true;
    }

    public override void Apply( GameObject target, float effectChance, float tickValue ) {
        if ( target.TryGetComponent( out EntityController entity ) ) {
            timeLeft = durationSeconds;
            isEffectActive = true;

            entity.EntityStatuses.AddEffect( this, effectChance );
        }

    }

    public override void UpdateEffect( GameObject target ) {
        SetEntity( target );

        wasActivated = true;

        entity.EntityHealth.isInvincible = true;
    }

    public override void Remove( GameObject target ) {
        SetEntity( target );

        isEffectActive = false;
        wasActivated = false;
        timeLeft = 0.0f;
        tickCooldown = 0.0f;

        entity.EntityHealth.isInvincible = false;
    }

    private void SetEntity( GameObject target ) {
        if ( entity == null ) {
            entity = target.GetComponent<EntityController>();
        }
    }
}
