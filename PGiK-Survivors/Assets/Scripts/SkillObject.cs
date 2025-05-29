using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour {
    [field: SerializeField] private Rigidbody2D rb;
    [field: SerializeField] private float moveSpeed = 5.0f;

    [field: SerializeField] private SkillSO skillData;

    private float dmg;
    private float effectChance;
    private float effectDamage;


    private StatusEffectSO statusEffect;
    private EntityController attackSource;
    private Vector2 origin;

    private bool doDestroy = false;
    private bool isLaunched = false;

    private void Update() {
        if ( PauseControl.IsGamePaused ) return;
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        if ( !isLaunched ) return;

        if ( collision.gameObject.TryGetComponent( out EntityController entity ) ) {

            if ( attackSource.gameObject.layer != entity.gameObject.layer ) {
                entity.EntityHealth.Damage( dmg );
                statusEffect?.Apply( entity.gameObject, effectChance, effectDamage );
                doDestroy = true;
            }
        }

        if ( doDestroy ) Destroy( gameObject, 0.1f );
    }

    public void Init( EntityController entity, Vector2 origin, SkillSO skillData ) {
        attackSource = entity;
        this.origin = origin;

        InitSkill();
        InitLifeTime();
    }

    private void InitSkill() {
        dmg = skillData.GetStatTotal( StatType.Damage );

        if ( skillData.statusEffect == null ) return;

        statusEffect = skillData.statusEffect;
        effectChance = skillData.GetStatTotal( statusEffect.effectType );
        effectDamage = skillData.GetEffectDamage( statusEffect.effectType );
    }

    private void InitLifeTime() {
        Destroy( gameObject, skillData.GetStatTotal( StatType.Lifetime ) );
    }

    public void Launch( Vector2 dir ) {
        rb.AddForce( dir * moveSpeed, ForceMode2D.Impulse );

        isLaunched = true;
    }
}
