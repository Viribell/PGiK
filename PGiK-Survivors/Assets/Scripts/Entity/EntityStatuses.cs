using System.Collections.Generic;
using UnityEngine;

public class EntityStatuses : MonoBehaviour, IEntityComponent {

    [field: SerializeField] private SerializableDictionary<EffectType, StatusEffectSO> cachedEffects;
    [field: SerializeField] private SerializableDictionary<EffectType, StatusEffectSO> enabledEffects;

    private float currentInterval = 0.0f;
    private float lastInterval = 0.0f;


    private EntityController entity;

    private void Awake() {
        cachedEffects = new SerializableDictionary<EffectType, StatusEffectSO>();
        enabledEffects = new SerializableDictionary<EffectType, StatusEffectSO>();
    }

    private void Update() {
        if ( PauseControl.IsGamePaused ) { return; }

        currentInterval += Time.deltaTime;
        if( currentInterval > lastInterval + 0.01f ) {
            UpdateEffects();
            lastInterval = currentInterval;
        }
    }

    public void AddEffect( StatusEffectSO effect, float effectChance ) {
        EffectType type = effect.effectType;
        
        if( !enabledEffects.ContainsKey( type ) ) {

            if ( ResistEffect( effect, effectChance ) ) return;
                
            enabledEffects[type] = CreateEffect( type, effect );

        } else {
            Debug.Log("Status: " + effect.effectType.ToString() + " already in effect");
        }
    }

    private bool ResistEffect( StatusEffectSO effect, float effectChance ) {
        if ( !effect.isNegative ) return false;

        float chanceRoll = Random.Range( 0.0f, 1.0f );

        float resistChance = entity.EntityStats.GetStatTotal( StatType.EffectResistance );

        effectChance -= effectChance * resistChance;

        if ( chanceRoll <= effectChance ) return false;
        else return true;
    }

    public void UpdateEffects() {
        List<EffectType> toRemove = new List<EffectType>();

        foreach(KeyValuePair<EffectType, StatusEffectSO> entry in enabledEffects) {
            entry.Value.Tick( gameObject, 0.01f );
            if ( entry.Value.CanBeRemoved() ) toRemove.Add(entry.Key);
        }

        foreach(EffectType type in toRemove) {
            RemoveEffect( type );
        }
    }

    public void RemoveEffect(EffectType type) {
        Debug.Log("Effect " + type.ToString() + " removed");
        
        if( enabledEffects.ContainsKey( type ) ) {
            enabledEffects[type].Remove( gameObject );

            enabledEffects.Remove(type);
        }
    }

    private StatusEffectSO CreateEffect( EffectType type, StatusEffectSO effectTemplate ) {
        if ( !cachedEffects.ContainsKey( type ) ) cachedEffects[type] = Instantiate( effectTemplate );

        if ( !cachedEffects[type].isEffectActive ) cachedEffects[type].Activate();

        return cachedEffects[type];
    }


    public void LoadEntityController( EntityController controller ) {
        entity = controller;
    }
}
