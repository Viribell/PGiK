using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//zmienic nazwe na IncreaseEffectSO i instancje tego beda regenup i attackspeedup 
[CreateAssetMenu( menuName = "Scriptable Objects/Status Effects/Stat Mod" )]
public class StatModEffectSO : StatusEffectSO {
    [Header( "StatModEffect Info" )]
    [field: SerializeField] public StatModifier statMod;

    private EntityController entity;

    private void Awake() {
        isEffectOverTime = false;
    }

    public override void Apply( GameObject target ) {
        if ( target.TryGetComponent( out EntityController entity ) ) {
            timeLeft = durationSeconds;
            isEffectActive = true;

            UpdateEffect( target );
            entity.EntityStatuses.AddEffect( this );
        }

    }

    public override void UpdateEffect( GameObject target ) {
        GetEntity( target );

        wasActivated = true;

        entity.EntityStats.AddStatMod( statMod, StatModHandlingOptions.NoDuplicateModAdd );
        entity.EntityStats.UpdateStat( statMod.affectedStat );
    }

    public override void Remove( GameObject target ) {
        GetEntity( target );

        isEffectActive = false;
        wasActivated = false;
        timeLeft = 0.0f;
        tickCooldown = 0.0f;

        entity.EntityStats.GetStat( statMod.affectedStat ).RemoveMod( statMod );
        entity.EntityStats.UpdateStat( statMod.affectedStat );
    }

    private EntityController GetEntity(GameObject target) {
        if( entity == null ) {
            entity = target.GetComponent<EntityController>();
        }

        return entity;
    }
}
