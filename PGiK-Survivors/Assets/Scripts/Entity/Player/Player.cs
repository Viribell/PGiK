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
        
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        if ( collision.gameObject.TryGetComponent( out EnemyController entity ) ) {
            TakeCollisionDamage( entity );
        }
    }

    private void TakeCollisionDamage( EnemyController entity ) {
        player.EntityHealth.Damage( EntityAttack.CalculateDamage(entity) );

        StatusEffectSO status = EntityAttack.ChooseEffect( entity );

        status.Apply( gameObject, entity.EntityStats.GetStatTotal( status.effectType ) );
    }

    public void LoadEntityController( EntityController controller ) {
        player = (PlayerController)controller;
    }
}
