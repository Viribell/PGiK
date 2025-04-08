using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header( "Player Health State" )]
    [field: SerializeField] private float currHealth;
    [field: SerializeField] private float maxHealth;

    [Header( "Event Channels" )]
    [field: SerializeField] private EventChannelSO OnHealthChanged;
    [field: SerializeField] private EventChannelSO OnPlayerDeath;

    private void Start() {
        maxHealth = PlayerControl.Instance.Player.GetStatTotal( StatType.Health );
        currHealth = maxHealth;
    }

    public void Damage( float dmgValue ) {
        currHealth -= dmgValue;

        if ( currHealth < 0 ) currHealth = 0;

        OnHealthChanged?.Raise();

        if ( IsDead() ) OnPlayerDeath?.Raise(); //albo event albo prostu funkcja do tego
    }

    public void Heal( float healValue ) {
        currHealth += healValue;

        if ( currHealth > maxHealth ) currHealth = maxHealth;

        OnHealthChanged?.Raise();
    }

    public void UpdateMaxHealth() {
        maxHealth = PlayerControl.Instance.Player.GetStatTotal( StatType.Health );

        OnHealthChanged?.Raise();
    }

    private bool IsDead() { return currHealth <= 0; }
}
