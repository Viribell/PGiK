using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour {
    [field: SerializeField] private Rigidbody2D rb;
    [field: SerializeField] private float moveSpeed = 5.0f;
    [field: SerializeField] private float lifeTimeAfterHit = 0.1f;
    [field: SerializeField] private bool destroyOnTrigger = true;

    private float dmg;
    private float effectChance;
    private float effectDamage;
    private float attackRange;

    private StatusEffectSO statusEffect;
    private EntityController attackSource;
    private Vector2 origin;

    private bool doDestroy = false;
    private bool isLaunched = false;

    private void FixedUpdate() {
        if ( Vector2.Distance( transform.position, origin ) >= attackRange ) {
            Destroy( gameObject, 0.1f );
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        if ( !isLaunched ) return;

        if ( collision.gameObject.TryGetComponent( out Destructible destructible ) ) {
            destructible.Destroy();
            doDestroy = true;
        }

        if ( collision.gameObject.TryGetComponent( out EntityController entity ) ) {

            if ( attackSource.gameObject.layer != entity.gameObject.layer ) {
                entity.EntityHealth.Damage( dmg );
                statusEffect?.Apply( entity.gameObject, effectChance, effectDamage );
                doDestroy = true;
            }
        }

        if ( doDestroy && destroyOnTrigger ) Destroy( gameObject, lifeTimeAfterHit );
        else doDestroy = false;
    }

    public void Init( EntityController entity, float dmg, Vector2 origin, SkillSO skillData ) {
        attackSource = entity;
        this.dmg = dmg;
        this.origin = origin;
        attackRange = skillData.GetStatTotal( StatType.AttackRange );
    }

    public void Init(EntityController entity, float dmg, Vector2 origin) {
        attackSource = entity;
        this.dmg = dmg;
        this.origin = origin;
        attackRange = attackSource.EntityStats.GetStatTotal( StatType.AttackRange );
    }

    public void InitEffect( StatusEffectSO statusEffect, float effectChance, float effectDamage ) {
        this.statusEffect = statusEffect;
        this.effectChance = effectChance;
        this.effectDamage = effectDamage;
    }

    public void Launch(Vector2 dir) {
        rb.AddForce( dir * moveSpeed, ForceMode2D.Impulse );

        isLaunched = true;
    }
}
