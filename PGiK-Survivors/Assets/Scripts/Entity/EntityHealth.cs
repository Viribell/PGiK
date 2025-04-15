using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour, IEntityComponent {
    [Header( "Entity Health State" )]
    [field: SerializeField] private float currHealth;
    [field: SerializeField] private float maxHealth;

    [Header( "Event Channels" )]
    [field: SerializeField] private EventChannelSO OnHealthChanged;
    [field: SerializeField] private EventChannelSO OnEntityDeath;

    private EntityController entityController;

    private void Start() {
        maxHealth = entityController.EntityStats.GetStatTotal( StatType.Health );
        currHealth = maxHealth;
    }

    public void Damage( float dmgValue ) {
        currHealth -= dmgValue;

        if ( currHealth < 0 ) currHealth = 0;

        OnHealthChanged?.Raise();

        if ( IsDead() ) OnEntityDeath?.Raise(); //somehow to be changed to work generally
    }

    public void Heal( float healValue ) {
        currHealth += healValue;

        if ( currHealth > maxHealth ) currHealth = maxHealth;

        OnHealthChanged?.Raise();
    }

    public void UpdateMaxHealth() {
        maxHealth = entityController.EntityStats.GetStatTotal( StatType.Health );

        OnHealthChanged?.Raise();
    }

    private bool IsDead() { return currHealth <= 0; }

    public void LoadEntityController( EntityController controller ) {
        entityController = controller;
    }
}
