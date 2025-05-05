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
        List<MagneticBehaviour> pickupables = FindMagneticBehaviours();

        foreach ( MagneticBehaviour pickup in pickupables ) {
            pickup.Gravitate( entity.transform, gravitySpeed );
        }

    }

    public override void Remove( GameObject target ) {
        GetEntity( target );

        isEffectActive = false;
        wasActivated = false;
        timeLeft = 0.0f;
        tickCooldown = 0.0f;
    }

    private List<MagneticBehaviour> FindMagneticBehaviours() {
        IEnumerable<MagneticBehaviour> peristentObjects = FindObjectsOfType<MonoBehaviour>( true ).OfType<MagneticBehaviour>();

        return new List<MagneticBehaviour>( peristentObjects );
    }

    private EntityController GetEntity( GameObject target ) {
        if ( entity == null ) {
            entity = target.GetComponent<EntityController>();
        }

        return entity;
    }
}
