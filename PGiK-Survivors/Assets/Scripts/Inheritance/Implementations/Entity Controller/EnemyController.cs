using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController {
    [field: Header( "Enemy Entity Info" )]
    [field: SerializeField] private Transform attackTarget;

    [field: Header( "Enemy Entity Config" )]
    [field: SerializeField] public Enemy Enemy { get; private set; }

    [field: Header( "Enemy Entity Debug Config" )]
    [field: SerializeField] private bool canWalk = true;

    [HideInInspector] public EnemySO EnemyData { get { return ( EnemySO )EntityData; } }

    private void Awake() {
        BaseUploadControllerToComponents();
        UploadControllerToComponents();
    }

    protected override void UploadControllerToComponents() {
        Enemy.LoadEntityController( this );
    }

    public override Vector2 GetMoveVector() {
        if ( !canWalk ) return Vector2.zero;

        if ( attackTarget == null ) return Vector2.zero;
        else return ( attackTarget.position - transform.position ).normalized;
    }
}
