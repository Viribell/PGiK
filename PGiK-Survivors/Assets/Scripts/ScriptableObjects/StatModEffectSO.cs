using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//zmienic nazwe na IncreaseEffectSO i instancje tego beda regenup i attackspeedup 
[CreateAssetMenu( menuName = "Scriptable Objects/Status Effects/Stat Mod" )]
public class StatModEffectSO : StatusEffectSO {
    [Header( "StatModEffect Info" )]
    [field: SerializeField] public StatModifier statMod;

    private EntityController entity;

    private void Awake() {
        isEffectOverTime = false;
    }

    public override void Apply( GameObject target, float effectChance ) {
        if ( target.TryGetComponent( out EntityController entity ) ) {
            timeLeft = durationSeconds;
            isEffectActive = true;

            entity.EntityStatuses.AddEffect( this, effectChance );
        }
    }

    public override void UpdateEffect( GameObject target ) {
        SetEntity( target );

        wasActivated = true;

        entity.EntityStats.AddStatMod( statMod, StatModHandlingOptions.NoDuplicateModAdd );
        entity.EntityStats.UpdateStat( statMod.affectedStat );
    }

    public override void Remove( GameObject target ) {
        SetEntity( target );

        isEffectActive = false;
        wasActivated = false;
        timeLeft = 0.0f;
        tickCooldown = 0.0f;

        entity.EntityStats.GetStat( statMod.affectedStat ).RemoveMod( statMod );
        entity.EntityStats.UpdateStat( statMod.affectedStat );
    }

    private void SetEntity(GameObject target) {
        if( entity == null ) {
            entity = target.GetComponent<EntityController>();
        }
    }
}
