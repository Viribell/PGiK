using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour {
    [Header( "Basic Info" )]
    [field: SerializeField] private Rigidbody2D rb;
    [field: SerializeField] private SkillSO skillData;
    [field: SerializeField] private AttackObject attackObject;

    [Header("Config")]
    [field: SerializeField] private bool destroyOnTrigger = false;
    [field: SerializeField] private bool isLaunchable = false;
    [field: SerializeField] private bool usesAttackObject = true;
    [field: SerializeField] private bool useRandomDir = true;
    [field: SerializeField] private float triggerDamageDelay = 0.2f;
    //[field: SerializeField] private bool focusEnemies = false;

    private float dmg;
    private float effectChance;
    private float effectDamage;
    private float moveSpeed;

    private StatusEffectSO statusEffect;
    private EntityController attackSource;
    private Vector2 origin;
    private float attackCooldown = 0.0f;

    private bool doDestroy = false;
    //private bool isLaunched = false;

    private float lastTriggerTime;

    private void Update() {
        if ( PauseControl.IsGamePaused ) return;

        if ( !usesAttackObject ) return;
        
        attackCooldown += Time.deltaTime;

        if ( attackCooldown >= skillData.GetStatTotal( StatType.AttackSpeed ) ) {
            Attack();
            attackCooldown = 0.0f;
        }
    }

    private void OnTriggerStay2D( Collider2D collision ) {
        if ( lastTriggerTime + triggerDamageDelay >= Time.time ) return; 

        if ( collision.gameObject.TryGetComponent( out EntityController entity ) ) {

            if ( attackSource.gameObject.layer != entity.gameObject.layer ) {
                entity.EntityHealth.Damage( CalculateDamage() );
                statusEffect?.Apply( entity.gameObject, effectChance, effectDamage );
                doDestroy = true;
                lastTriggerTime = Time.time;
            }
        }

        if ( doDestroy && destroyOnTrigger ) Destroy( gameObject, 0.1f );
        else doDestroy = false;
    }

    private void Attack() {
        float strikes = skillData.GetStatTotal( StatType.Strikes );

        for ( int i = 0; i < Mathf.Round( strikes ); i++ ) {
            Vector2 attackDir = GetAttackDir( (int)Mathf.Round( strikes ), i + 1 );
            float prefabAngle = Mathf.Atan2( attackDir.y, attackDir.x ) * Mathf.Rad2Deg + attackObject.transform.rotation.eulerAngles.z;
            Quaternion prefabRotation = Quaternion.Euler( 0, 0, prefabAngle );

            SpawnAttack( attackObject, attackDir, prefabRotation );
        }
    }

    private Vector2 GetAttackDir(int strikes = -1, int attackNumber = -1) {
        float angle = 0.0f;
        
        if( useRandomDir ) {
            return GetRandomDir();
        } else {
            if ( attackNumber < 0 || strikes < 0 ) return new Vector2();

            float step = 360.0f / strikes;
            angle = step * attackNumber;

            return new Vector2( Mathf.Cos( angle * Mathf.Deg2Rad ), Mathf.Sin( angle * Mathf.Deg2Rad ) );
        }
    }

    private Vector2 GetRandomDir() {
        float angle = Random.Range( 0.0f, 360.0f );
        return new Vector2( Mathf.Cos( angle * Mathf.Deg2Rad ), Mathf.Sin( angle * Mathf.Deg2Rad ) );
    }

    private void SpawnAttack( AttackObject attackPrefab, Vector2 attackDir, Quaternion rotation ) {
        AttackObject attack = Instantiate( attackPrefab, transform.position, rotation );

        //Attack Size Change
        Vector3 attackScale = attack.transform.localScale;
        attackScale.y = attackScale.x = skillData.GetStatTotal( StatType.AttackSize );
        attack.transform.localScale = attackScale;

        //Attack Damage
        attack.Init( attackSource, CalculateDamage(), transform.position, skillData );
        attack.InitEffect( statusEffect, effectChance, effectDamage );

        //Launch Attack
        attack.Launch( attackDir );
    }

    public float CalculateDamage() {
        float chanceRoll = Random.Range( 0.0f, 1.0f );

        float damage = skillData.GetStatTotal( StatType.Damage );

        if ( chanceRoll <= skillData.GetStatTotal( StatType.CritChance ) ) {
            damage += damage * skillData.GetStatTotal( StatType.CritBonus );
        }

        return damage;
    }

    #region Initiate

    public void Init( EntityController entity, Vector2 origin, SkillSO skillData ) {
        attackSource = entity;
        this.origin = origin;
        this.skillData = skillData;

        InitSkill();
        InitLifeTime();
        if ( !usesAttackObject ) InitMisc();

        if ( isLaunchable ) Launch();
    }

    private void InitSkill() {
        dmg = skillData.GetStatTotal( StatType.Damage );
        moveSpeed = skillData.GetStatTotal( StatType.Speed );

        if ( skillData.statusEffect == null ) return;

        statusEffect = skillData.statusEffect;
        effectChance = skillData.GetStatTotal( statusEffect.effectType );
        effectDamage = skillData.GetEffectDamage( statusEffect.effectType );
    }

    private void InitMisc() {
        Vector3 attackScale = transform.localScale;
        attackScale.y = attackScale.x = skillData.GetStatTotal( StatType.AttackSize );
        transform.localScale = attackScale;

        if( useRandomDir ) ChangeRotation( GetRandomDir() );
    }

    private void ChangeRotation( Vector2 dir ) {
        float prefabAngle = Mathf.Atan2( dir.y, dir.x ) * Mathf.Rad2Deg + transform.rotation.eulerAngles.z;
        Quaternion prefabRotation = Quaternion.Euler( 0, 0, prefabAngle );
        transform.localRotation = prefabRotation;
    }

    private void InitLifeTime() {
        Destroy( gameObject, skillData.GetStatTotal( StatType.Lifetime ) );
    }

    public void Launch() {
        Vector2 dir = GetRandomDir();

        if ( useRandomDir ) ChangeRotation( dir );
        rb.AddForce( dir * moveSpeed, ForceMode2D.Impulse );

        //isLaunched = true;
    }
    #endregion 
}
