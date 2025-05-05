using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController {
    [field: Header( "Enemy Entity Info" )]
    [field: SerializeField] private PlayerController attackTarget;

    [field: Header( "Enemy Entity Config" )]
    [field: SerializeField] public Enemy Enemy { get; private set; }

    [field: Header( "Enemy Entity Debug Config" )]
    [field: SerializeField] private bool canWalk = true;

    [HideInInspector] public EnemySO EnemyData { get { return ( EnemySO )EntityData; } }

    private void Start() {
        attackTarget = RefCacheControl.Instance.Player;
    }

    protected override void UploadControllerToComponents() {
        Enemy.LoadEntityController( this );
    }

    public override Vector2 GetMoveVector() {
        if ( attackTarget == null || !canWalk ) return Vector2.zero;

        return ( attackTarget.transform.position - transform.position ).normalized;
    }

    #region Events
    public override void OnEntityDeath() {
        QuestControl.Instance.UpdateKillQuests( this );
        Enemy.Die();
    }

    public override void OnHealthChanged() {}

    public override void OnDamaged( float value ) {}

    public override void OnHealed( float value ) {}
    #endregion
}
