using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : EntityController {
    [field: Header( "Enemy Entity Info" )]
    [field: SerializeField] private PlayerController attackTarget;

    [field: Header( "Enemy Entity Config" )]
    [field: SerializeField] public Enemy Enemy { get; private set; }

    [field: Header( "Enemy Entity Debug Config" )]
    [field: SerializeField] private bool canWalk = true;

    [HideInInspector] public EnemySO EnemyData { get { return ( EnemySO )EntityData; } }

    private UnityAction DeathAction;

    private void Start() {
        attackTarget = RefCacheControl.Instance.Player;
        LoadStadiumMods();
    }

    private void LoadStadiumMods() {
        List<StatModifier> mods = LevelControl.Instance.stadiumMods;

        if ( mods == null ) return;

        EntityStats.AddStatMod( mods, StatModHandlingOptions.NoDuplicateModAdd );

        foreach(StatModifier mod in mods) {
            EntityStats.UpdateStat( mod.affectedStat );
        }

        EntityHealth.UpdateMaxHealth();
        EntityHealth.UpdateCurrentHealth();
    }

    protected override void UploadControllerToComponents() {
        Enemy.LoadEntityController( this );
    }

    public override Vector2 GetMoveVector() {
        if ( attackTarget == null || !canWalk ) return Vector2.zero;

        return ( attackTarget.transform.position - transform.position ).normalized;
    }

    public void SetDeathAction(UnityAction action) {
        DeathAction = action;
    }

    #region Events
    public override void OnEntityDeath() {
        QuestControl.Instance.UpdateKillQuests( this );
        DeathAction?.Invoke();
        Enemy.Die();
    }

    public override void OnHealthChanged() {}

    public override void OnDamaged( float value ) {}

    public override void OnHealed( float value ) {}
    #endregion
}
