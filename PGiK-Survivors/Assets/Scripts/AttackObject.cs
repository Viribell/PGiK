using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour {
    [field: SerializeField] private Rigidbody2D rb;
    [field: SerializeField] private float moveSpeed = 5.0f;

    private float dmg;
    private float effectChance;

    private StatusEffectSO statusEffect;
    private EntityController attackSource;
    private Vector2 origin;

    private bool doDestroy = false;
    private bool isLaunched = false;

    private void FixedUpdate() {
        if ( Vector2.Distance( transform.position, origin ) >= attackSource.EntityStats.GetStatTotal( StatType.AttackRange ) ) {
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
                statusEffect?.Apply( entity.gameObject, effectChance );
                doDestroy = true;
            }
        }

        if ( doDestroy ) Destroy( gameObject, 0.1f );
    }

    public void Init(EntityController entity, float dmg, Vector2 origin) {
        attackSource = entity;
        this.dmg = dmg;
        this.origin = origin;
    }

    public void InitEffect( StatusEffectSO statusEffect, float effectChance ) {
        this.statusEffect = statusEffect;
        this.effectChance = effectChance;
    }

    public void Launch(Vector2 dir) {
        rb.AddForce( dir * moveSpeed, ForceMode2D.Impulse );

        isLaunched = true;
    }
}
