using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour {
    [Header("Basic Entity Data")]
    [field: SerializeField] public EntityType entityType;
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public EntitySO EntityData { get; set; }
    [field: SerializeField] public EntitySO DefaultEntityData { get; set; }
    [field: SerializeField] public EntityAudioSetSO EntityAudio { get; set; }

    [field: Header( "Basic Entity Config" )]
    [field: SerializeField] public EntityStats EntityStats { get; private set; }
    [field: SerializeField] public EntityStatuses EntityStatuses { get; private set; }
    [field: SerializeField] public EntityHealth EntityHealth { get; private set; }
    [field: SerializeField] public EntityMove EntityMove { get; private set; }
    [field: SerializeField] public EntityAttack EntityAttack { get; private set; }
    [field: SerializeField] public EntitySkills EntitySkills { get; private set; }

    private void Awake() {
        BaseUploadControllerToComponents();
        UploadControllerToComponents();
    }

    public void UploadEntityData(EntitySO data) {
        EntityData = data;
        ReloadEntityData();
    }

    public void ReloadEntityData() {
        BaseReloadData();
        ReloadData();
    }
    
    private void BaseReloadData() {
        EntityStats.ReloadEntityData();
        EntityStatuses.ReloadEntityData();
        EntityHealth.ReloadEntityData();
        EntityMove.ReloadEntityData();
        EntityAttack.ReloadEntityData();

        EntitySkills?.ReloadEntityData();
    }

    private void BaseUploadControllerToComponents() {
        EntityStats.LoadEntityController( this );
        EntityStatuses.LoadEntityController( this );
        EntityHealth.LoadEntityController( this );
        EntityMove.LoadEntityController( this );

        EntitySkills?.LoadEntityController( this );
        EntityAttack?.LoadEntityController( this );
    }

    protected abstract void UploadControllerToComponents();
    protected abstract void ReloadData();
    public abstract Vector2 GetMoveVector();

    public abstract void OnEntityDeath();
    public abstract void OnDamaged( float value );
    public abstract void OnHealed( float value );
    public abstract void OnHealthChanged();
    public abstract void OnStatEdgeCase( StatType type );
}
