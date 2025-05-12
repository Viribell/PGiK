using System.Collections;
using UnityEngine;

public class EntityHealth : MonoBehaviour, IEntityComponent {
    [Header( "Entity Health State" )]
    [field: SerializeField][Lock] private float currHealth;
    [field: SerializeField][Lock] private float maxHealth;
    [field: SerializeField][Lock] private float lastTimeDamaged;

    [Header( "Entity Health Config" )]
    [field: SerializeField] private bool usesInvincibilityTime = false;
    [field: SerializeField] private float invincibilityTime = 2.0f; //seconds
    [field: SerializeField] private float regenDelay = 1.5f; //seconds, maybe to be moved as parts of Stats
    [field: SerializeField] public bool isInvincible = false;

    private EntityController entityController;

    private WaitForSeconds waitFor1Second;
    private Coroutine regenRoutine;

    private void Start() {
        maxHealth = entityController.EntityStats.GetStatTotal( StatType.Health );
        currHealth = maxHealth;

        waitFor1Second = new WaitForSeconds( 1.0f );
    }

    //it prolly should be in Update(), but for now it will use this
    private void FixedUpdate() {
        if( entityController.EntityStats.GetStatTotal( StatType.Regeneration ) > 0.0f ) StartHPRegeneration();
    }

    #region HPRegen

    private void StartHPRegeneration() {
        if ( IsFullHealth() || regenRoutine != null || lastTimeDamaged + regenDelay >= Time.time) return;

        regenRoutine = StartCoroutine( RegenerateHP() );
    }

    private IEnumerator RegenerateHP() {
        while( !IsFullHealth() ) {
            Heal( entityController.EntityStats.GetStatTotal( StatType.Regeneration ) );

            yield return waitFor1Second;
        }

        regenRoutine = null;
    }

    #endregion

    #region HealthLogic

    public void Damage( float dmgValue ) {
        if ( ( usesInvincibilityTime && IsInvincible() ) || isInvincible ) return;

        lastTimeDamaged = Time.time;

        dmgValue = Mathf.Max(1, dmgValue - entityController.EntityStats.GetStatTotal( StatType.Defense ) ); //dunno if have defense as flat or percentage based
        //tho with percentage its trickier to get damaged more with minus defense

        currHealth = ( float )System.Math.Round( Mathf.Max(0, currHealth - dmgValue), 4 );

        //Stop regen after dmg
        if( regenRoutine != null ) {
            StopCoroutine( regenRoutine );
            regenRoutine = null;
        }

        entityController.OnDamaged( dmgValue );

        if ( IsDead() ) entityController.OnEntityDeath();
    }

    public void Heal( float healValue ) {
        currHealth = ( float )System.Math.Round( Mathf.Min( currHealth + healValue, maxHealth ), 4 );

        entityController.OnHealed( healValue );
    }

    #endregion

    #region MiscFunctions

    public void UpdateMaxHealth() {
        maxHealth = entityController.EntityStats.GetStatTotal( StatType.Health );

        entityController.OnHealthChanged();
    }

    public void UpdateCurrentHealth() {
        currHealth = maxHealth;
    }

    private bool IsInvincible() { return lastTimeDamaged + invincibilityTime >= Time.time; }
    private bool IsDead() { return currHealth <= 0; }
    private bool IsFullHealth() { return currHealth >= maxHealth; }

    public void LoadEntityController( EntityController controller ) {
        entityController = controller;
    }

    #endregion
}
