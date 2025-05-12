using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttack : MonoBehaviour, IEntityComponent {

    [Header( " Attack Info " )]
    [field: SerializeField] private Transform attackPivot;
    [field: SerializeField] private Transform attackSpawn;

    [Header( "General Config" )]
    [field: SerializeField] private bool usesAttackObject = false;

    [Header( "Attack State" )]
    [field: SerializeField] private List<StatusEffectSO> possibleEffects;

    private EntityController entity;

    private float attackCooldown = 0.0f;

    private void Update() {
        if ( PauseControl.IsGamePaused ) { return; }

        if ( !usesAttackObject ) return;

        attackCooldown += Time.deltaTime;

        if ( attackCooldown >= entity.EntityStats.GetStatTotal( StatType.AttackSpeed ) ) {
            Attack();
            attackCooldown = 0.0f;
        }
    }

    private void Attack() {
        AttackObject attackPrefab = entity.EntityData.attackPrefab;
        float strikes = entity.EntityStats.GetStatTotal( StatType.Strikes );

        Vector2 attackDir = ( attackSpawn.position - attackPivot.position ).normalized;
        float prefabAngle = Mathf.Atan2( attackDir.y, attackDir.x ) * Mathf.Rad2Deg + attackPrefab.transform.rotation.eulerAngles.z;

        float startAngle = strikes == 1 ? 0.0f : (-entity.EntityData.spreadAngle / 2.0f);
        float angleStep = 0.0f;

        if ( ( strikes - 1 ) >= 1 ) angleStep = entity.EntityData.spreadAngle / ( strikes - 1 );

        for (int i = 0; i < Mathf.Round(strikes); i++ ) {
            float rotationAngle = startAngle + angleStep * i;
            Quaternion angleQuaternion = Quaternion.Euler( 0, 0, rotationAngle );
            Quaternion prefabRotation = Quaternion.Euler( 0, 0, prefabAngle + rotationAngle );

            SpawnAttack( attackPrefab, ( angleQuaternion * attackDir ).normalized, prefabRotation );
        }
    }

    private void SpawnAttack( AttackObject attackPrefab, Vector2 attackDir, Quaternion rotation ) {
        AttackObject attack = Instantiate( attackPrefab, attackSpawn.position, rotation );

        //Attack Size Change
        Vector3 attackScale = attack.transform.localScale;
        attackScale.y = attackScale.x = entity.EntityStats.GetStatTotal( StatType.AttackSize );
        attack.transform.localScale = attackScale;

        //Attack Damage And Origin
        attack.Init( entity, CalculateDamage( entity ), attackSpawn.position );

        //Attack Status Effect Choice
        StatusEffectSO chosenEffect = ChooseEffect();
        float effectChance = chosenEffect == null ? 0 : entity.EntityStats.GetStatTotal( chosenEffect.effectType );
        float effectDamage = chosenEffect == null ? 0 : entity.EntityStats.GetEffectDamage( chosenEffect.effectType );
        attack.InitEffect( chosenEffect, effectChance, effectDamage );

        //Launch Attack
        attack.Launch( attackDir );
    }

    private StatusEffectSO ChooseEffect() {
        if ( possibleEffects == null || possibleEffects.Count <= 0 ) return null;
        if ( possibleEffects.Count == 1 ) return possibleEffects[0];

        int chanceRoll = Random.Range( 0, possibleEffects.Count );

        return possibleEffects[ chanceRoll ];
    }

    #region StaticAttacker
    public static StatusEffectSO ChooseEffect(EntityController attacker) {
        List<StatusEffectSO> possibleEffects = attacker.EntityData.possibleEffects;

        if ( possibleEffects == null || possibleEffects.Count <= 0 ) return null;
        if ( possibleEffects.Count == 1 ) return possibleEffects[0];

        int chanceRoll = Random.Range( 0, possibleEffects.Count );

        return possibleEffects[chanceRoll];
    }

    public static float CalculateDamage(EntityController attacker) {
        float chanceRoll = Random.Range( 0.0f, 1.0f );

        float damage = attacker.EntityStats.GetStatTotal( StatType.Damage );

        if ( chanceRoll <= attacker.EntityStats.GetStatTotal( StatType.CritChance ) ) {
            damage += damage * attacker.EntityStats.GetStatTotal( StatType.CritBonus );
        }

        return damage;
    }
    #endregion

    public void LoadEntityController( EntityController controller ) {
        entity = controller;

        possibleEffects = entity.EntityData.GetPossibleEffects();
    }
}
