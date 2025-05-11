using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu( menuName = "Scriptable Objects/Status Effects/Magnet" )]
public class MagnetEffectSO : StatusEffectSO {
    [Header( "MagnetEffect Info" )]
    [field: SerializeField] public float gravitySpeed = 10.0f;


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
        List<MagneticBehaviour> pickupables = FindMagneticBehaviours();

        foreach ( MagneticBehaviour pickup in pickupables ) {
            pickup.Gravitate( entity.transform, gravitySpeed );
        }
    }

    public override void Remove( GameObject target ) {
        SetEntity( target );

        isEffectActive = false;
        wasActivated = false;
        timeLeft = 0.0f;
        tickCooldown = 0.0f;
    }

    private List<MagneticBehaviour> FindMagneticBehaviours() {
        IEnumerable<MagneticBehaviour> peristentObjects = FindObjectsOfType<MonoBehaviour>( true ).OfType<MagneticBehaviour>();

        return new List<MagneticBehaviour>( peristentObjects );
    }

    private void SetEntity( GameObject target ) {
        if ( entity == null ) {
            entity = target.GetComponent<EntityController>();
        }
    }
}
