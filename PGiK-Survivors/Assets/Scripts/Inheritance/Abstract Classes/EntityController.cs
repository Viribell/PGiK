using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour {
    [Header("Basic Entity Data")]
    [field: SerializeField] public EntityType entityType;
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public EntitySO EntityData { get; set; }

    [field: Header( "Basic Entity Config" )]
    [field: SerializeField] public EntityStats EntityStats { get; private set; }
    [field: SerializeField] public EntityStatuses EntityStatuses { get; private set; }
    [field: SerializeField] public EntityHealth EntityHealth { get; private set; }
    [field: SerializeField] public EntityMove EntityMove { get; private set; }

    protected void BaseUploadControllerToComponents() {
        EntityStats.LoadEntityController( this );
        EntityStatuses.LoadEntityController( this );
        EntityHealth.LoadEntityController( this );
        EntityMove.LoadEntityController( this );
    }

    //public virtual EntityHealth Health() { } 
    //public virtual EntityMove Move() { } 

    protected abstract void UploadControllerToComponents();
    public abstract Vector2 GetMoveVector();
}
