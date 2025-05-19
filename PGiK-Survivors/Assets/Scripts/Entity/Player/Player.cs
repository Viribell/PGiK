using UnityEngine;

public class Player : MonoBehaviour, IEntityComponent {
    private PlayerController player;

    private void Start() {
        if ( player.EntityData != null ) Init();
    }

    private void Init() {
        if ( player.EntityData.sprite != null && player.SpriteRenderer != null ) player.SpriteRenderer.sprite = player.EntityData.sprite;
    }

    public void Die() {
        Destroy( gameObject, 1 );
    }

    private void OnCollisionStay2D( Collision2D collision ) {
        if ( collision.gameObject.TryGetComponent( out EnemyController entity ) ) {
            TakeCollisionDamage( entity );
        }
    }

    private void TakeCollisionDamage( EnemyController entity ) {
        player.EntityHealth.Damage( EntityAttack.CalculateDamage(entity) );

        StatusEffectSO status = EntityAttack.ChooseEffect( entity );
        float statusChance = status == null ? 0 : entity.EntityStats.GetStatTotal( status.effectType );
        float effectDamage = status == null ? 0 : entity.EntityStats.GetEffectDamage( status.effectType );

        status?.Apply( gameObject, statusChance, effectDamage );
    }

    public void LoadEntityController( EntityController controller ) {
        player = (PlayerController)controller;
    }

    public void ReloadEntityData() {
        Init();
    }
}
